﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwapi.ExtractsManagement.Core.Model.Source.Hts;
using Dwapi.SharedKernel.Interfaces;

namespace Dwapi.ExtractsManagement.Core.Interfaces.Repository.Hts
{ [Obsolete("Class is obsolete,use ITempHtsClientsExtractRepository")]
    public interface ITempHTSClientExtractRepository : IRepository<TempHTSClientExtract,Guid>
    {
        Task Clear();
        bool BatchInsert(IEnumerable<TempHTSClientExtract> extracts);
    }
}
