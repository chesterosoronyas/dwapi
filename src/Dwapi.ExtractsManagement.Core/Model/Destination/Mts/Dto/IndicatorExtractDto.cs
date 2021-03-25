using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;

namespace Dwapi.ExtractsManagement.Core.Model.Destination.Mts.Dto
{
    public class IndicatorExtractDto
    {
        public Guid Id { get; set; }
        public string Indicator { get; set; }
        public string Description { get; set; }
        public string IndicatorValue { get; set; }
        public DateTime? IndicatorDate { get; set; }
        public DateTime DateCreated { get; set; }
        public int Rank { get; set; }
        public string TimeAgo => GetTimeAgo();
        public string Status { get; set; }

        private string GetTimeAgo()
        {
            return DateCreated.Humanize(false);
        }

        public static List<IndicatorExtractDto> GenerateValidations(List<IndicatorExtractDto> indicatorExtractDtos)
        {
            var list = new List<IndicatorExtractDto>();
            foreach (var indicatorExtractDto in indicatorExtractDtos)
            {
                indicatorExtractDto.Validate(indicatorExtractDtos);
                list.Add(indicatorExtractDto);
            }
            return list;
        }

        private void Validate(List<IndicatorExtractDto> indicatorExtractDtos)
        {
            if (Indicator =="RETENTION_ON_ART_VL_1000_12_MONTHS")
            {
                Status = GetStatus(
                    indicatorExtractDtos,
                    Indicator,
                    "RETENTION_ON_ART_12_MONTHS",
                    "RETENTION_ON_ART_VL_1000_12_MONTHS Subset of RETENTION_ON_ART_12_MONTHS Cannot be greater than RETENTION_ON_ART_12_MONTHS Please check your data. Value cannot be greater than RETENTION_ON_ART_12_MONTHS");
            }

            if (Indicator =="HTS_TESTED_POS")
            {
                Status = GetStatus(
                    indicatorExtractDtos,
                    Indicator,
                    "HTS_TESTED",
                    "HTS_TESTED_POS Subset of HTS_TESTED Cannot be greater than HST_TESTED Please check your data. Value cannot be greater than HST_TESTED");
            }

            if (Indicator =="HTS_INDEX_POS")
            {
                Status = GetStatus(
                    indicatorExtractDtos,
                    Indicator,
                    "HTS_INDEX",
                    "HTS_INDEX_POS Subset of HTS_INDEX Cannot be greater than HTS_INDEX Please check your data. Value cannot be greater than HTS_INDEX");
            }

            if (Indicator =="HTS_TESTED_POS")
            {
                Status = GetStatus(
                    indicatorExtractDtos,
                    Indicator,
                    "TX_NEW",
                    "Please check for linkage to other facilities or Newly initiated clients tested elsewhere",
                    LogicCalc.NEQ);
            }

            if (Indicator =="TX_CURR")
            {
                Status = GetStatus(
                    indicatorExtractDtos,
                    Indicator,
                    "TX_NEW",
                    "Please check your data  TX_CURR Should be greater than or equals to TX_NEW TX_CURR cannot be less than TX_NEW",
                    LogicCalc.NEQ);
            }

            if (Indicator =="TX_PVLS")
            {
                Status = GetStatus(
                    indicatorExtractDtos,
                    Indicator,
                    "TX_CURR",
                    "TX_PVLS Subset of TX_CURR Cannot be greater than TX_CURR	Please check your data .Value cannot be greater than TX_CURR",
                    LogicCalc.GT);
            }
            if (Indicator =="MMD")
            {
                Status = GetStatus(
                    indicatorExtractDtos,
                    Indicator,
                    "TX_CURR",
                    "MMD Subset of TX_CURR Cannot be greater than TX_CURR Please check your data .Value cannot be greater than TX_CURR",
                    LogicCalc.GT);
            }
        }

        private string GetStatus(List<IndicatorExtractDto> indicatorExtractDtos,string indiA,string indiB,string msg,LogicCalc cal=LogicCalc.GT)
        {
            var status = "";
            if (Indicator == indiA)
            {
                var checkAgainst = indicatorExtractDtos.FirstOrDefault(x => x.Indicator == indiB);
                if (null != checkAgainst)
                {
                    if (int.TryParse(IndicatorValue, out var intA) &&
                        int.TryParse(checkAgainst.IndicatorValue, out var intB))
                    {
                        if (cal == LogicCalc.GT)
                        {
                            status = intA > intB
                                ? msg
                                : "";
                        }

                        if (cal == LogicCalc.GTEQ)
                        {
                            status = intA >= intB
                                ? msg
                                : "";
                        }

                        if (cal == LogicCalc.EQ)
                        {
                            status = intA == intB
                                ? msg
                                : "";
                        }

                        if (cal == LogicCalc.NEQ)
                        {
                            status = intA != intB
                                ? msg
                                : "";
                        }
                        if (cal == LogicCalc.LT)
                        {
                            status = intA < intB
                                ? msg
                                : "";
                        }

                        if (cal == LogicCalc.LTEQ)
                        {
                            status = intA <= intB
                                ? msg
                                : "";
                        }
                    }
                }
            }
            return status;
        }
    }

    enum LogicCalc
    {
        GT,EQ,NEQ,LT,GTEQ,LTEQ
    }


}
