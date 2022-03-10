const { doesNotThrow } = require("assert");

Cypress.on("uncaught:exception", (err, runnable) => {
    // returning false here prevents Cypress from
    // failing the test
    return false;
});
before(function () {
    //something ...
    cy.visit("/#/mnch", { timeout: 100000 });
});

it("Send MNCH data to the Data warehouse", () => {
    cy.wait(3000);
    cy.contains("Send to Warehouse").click({ force: true });
    //cy.contains("Load from EMR").click({ force: true });

    cy.intercept("GET", "/api/Mnch/status/**", (req) => {
        console.log(req);
    }).as("status");

    cy.wait("@status", {
        requestTimeout: 1000000,
        defaultCommandTimeout: 1000000,
    })
        .its("response.statusCode")
        .should("equal", 200);
});
