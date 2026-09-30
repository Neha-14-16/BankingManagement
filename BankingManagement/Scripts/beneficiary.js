document.addEventListener("DOMContentLoaded", function () {

    var bankDropdown =
        document.getElementById("BankBranchId");

    var branchDropdown =
        document.getElementById("BranchDropdown");

    var ifscInput =
        document.getElementById("IFSCCode");

    if (!bankDropdown ||
        !branchDropdown ||
        !ifscInput) {
        return;
    }

    bankDropdown.addEventListener("change", function () {

        var selectedBankId =
            bankDropdown["value"];

        var options =
            branchDropdown.getElementsByTagName("option");

        branchDropdown["value"] = "";
        ifscInput["value"] = "";

        for (var i = 0; i < options.length; i++) {

            var option = options[i];

            if (option.getAttribute("value") === selectedBankId) {

                branchDropdown["value"] =
                    selectedBankId;

                var ifsc =
                    option.getAttribute("data-ifsc");

                if (ifsc) {
                    ifscInput["value"] = ifsc;
                }

                break;
            }
        }

    });

});