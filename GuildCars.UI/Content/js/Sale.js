class Workflow {
    constructor() {
        this._customerId = "";
        this._customer = {};
        this._tradeIn = {};
        this._purchased = {};

        this.cacheDom();
        this.initTabs();
        this.initEventListeners();
    }

    cacheDom() {
        this._allTabs = document.querySelectorAll("button.nav-link");

        this._findCustomerBtn = document.querySelector(".customer-info-form__btn");
        this._customerInfoDisplay = document.querySelector(".customer-info-container");
        this._selectCustomerBtn = document.querySelector(".select-customer-btn");

        this._tradeInTab = document.querySelector("#trade-in-tab");
        this._tradeInContainer = document.querySelector(".trade-in-container");
        this._selectedTradeInDetails = document.querySelector(".selected-trade-in-details");

        this._purchasedTab = document.querySelector("#purchased-vehicle-tab");
        this._availableVehiclesList = document.querySelector(".available-vehicles-list");
        this._selectedVehicleDetails = document.querySelector(".selected-vehicle-details");

        this._transactionTab = document.querySelector("#transaction-tab");
        this._transactionContainer = document.querySelector(".transaction__container");
        this._transactionCustomerInfoContainer = document.querySelector(".transaction__customer-info-container");
        this._transactionAllVehicleInfoContainer = document.querySelector(".transaction__all-vehicle-info-container");
        this._transactionName = document.querySelector(".transaction-customer-info__name");
        this._transactionEmail = document.querySelector(".transaction-customer-info__email");
        this._transactionPhone = document.querySelector(".transaction-customer-info__phone");
        this._transactionAddress1 = document.querySelector(".transaction-customer-info__address1");
        this._transactionAddress2 = document.querySelector(".transaction-customer-info__address2");
        this._transactionCity = document.querySelector(".transaction-customer-info__city");
        this._transactionState = document.querySelector(".transaction-customer-info__state");
        this._transactionZip = document.querySelector(".transaction-customer-info__zip");
        this._transactionTradeInMakeModel = document.querySelector(".transaction-tradein__makemodel");
        this._transactionTradeInVin = document.querySelector(".transaction-tradein__vin");
        this._transactionTradeInValue = document.querySelector(".transaction-tradein__value");
        this._transactionPurchaseMakeModel = document.querySelector(".transaction-purchase__makemodel");
        this._transactionPurchaseVin = document.querySelector(".transaction-purchase__vin");
        this._transactionPurchaseSalePrice = document.querySelector(".transaction-purchase__saleprice");
        this._transactionPurchaseBalanceAfterTradeIn = document.querySelector(".transaction-purchase__balance-after-trade-in");

        this._purchaseMethodSelector = document.querySelector(".transaction__purchase-method-selector");
        this._cashPane = document.querySelector(".cash__container");
        this._bankPane = document.querySelector(".bank-finance__container");
        this._dealerPane = document.querySelector(".dealer-finance__container");

        this._cashPurchaseAmount = document.querySelector("#payment-amount");
        this._cashBtn = document.querySelector(".cash-btn");

        this._dealerDownpayment = document.querySelector("#dealer-downpayment");
        this._dealerInterestRate = document.querySelector("#dealer-interest-rate");
        this._dealerLoanLength = document.querySelector("#dealer-loan-length");
        this._dealerBtn = document.querySelector(".dealer-btn");

        this._bankDownpayment = document.querySelector("#bank-downpayment");
        this._bankLoanLength = document.querySelector("#bank-loan-length");
        this._bankInterestRate = document.querySelector("#bank-interest-rate");
        this._bankApprovalAmount = document.querySelector("#bank-approval-amount");
        this._bankApprovalLetter = document.querySelector("#bank-approval-letter");
        this._bankBtn = document.querySelector(".bank-btn");
    }

    initEventListeners() {
        this._findCustomerBtn.addEventListener("click", async () => await this.fetchCustomer());
        this._selectCustomerBtn.addEventListener("click", () => this.handleSelectCustomerBtnClick());
        this._tradeInTab.addEventListener("shown.bs.tab", async () => await this.fetchTradeInVehicles());
        this._purchasedTab.addEventListener("shown.bs.tab", async () => await this.fetchAvailableVehicles());
        this._transactionTab.addEventListener("shown.bs.tab", async () => await this.handleTransactionTabClick());
        this._purchaseMethodSelector.addEventListener("change", e => this.handlePurchaseMethodSelection(e));

        this._cashBtn.addEventListener("click", async () => await this.handleCashSaleClick());
        this._dealerBtn.addEventListener("click", async () => await this.handleDealerSaleClick());
        this._bankBtn.addEventListener("click", async () => await this.handleBankSaleClick());
    }

    initTabs() {
        const tabElements = document.querySelectorAll("#saleTabs button");
        const tabArr = Array.from(tabElements);
        tabArr.forEach(tabElement => {
            const tab = new bootstrap.Tab(tabElement);

            tabElement.addEventListener("click", () => tab.show())
        })
    }

    async fetchCustomer() {
        const firstName = document.querySelector(".customer-info-form input[aria-labelledby='first-name-label']").value;
        const lastName = document.querySelector(".customer-info-form input[aria-labelledby='last-name-label']").value;
        const email = document.querySelector(".customer-info-form input[aria-labelledby='email-label']").value;

        try {
            const response = await fetch(`https://localhost:44346/Sale/GetCustomerInfo`, {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ firstName, lastName, email })
            });
            if (!response.ok) {
                throw new Exception();
            }

            const customerInfo = await response.text();
            this._customerInfoDisplay.innerHTML = customerInfo;
        }
        catch {
            alert("Unable to find customer - network error, try again later");
        }
        finally {
            if (this.wasCustomerInfoFound()) {
                this._selectCustomerBtn.style.display = "block";
                this._customerId = document.querySelector(".customer-info__id").value;
            }
            else {
                this.resetForm();
            }
        }
    }

    wasCustomerInfoFound() {
        return document.querySelector(".customer-info__id");
    }

    resetForm() {
        this._selectCustomerBtn.style.display = "none";
        this.disableTabs();
        this._customerId = "";
        this._tradeInId = "";
        this._purchasedVehicleId = "";
        this._tradeInContainer.innerHTML = "";
        this._availableVehiclesList.innerHTML = "";
        this._selectedTradeInDetails.innerHTML = "";
        this._selectedVehicleDetails.innerHTML = "";
        this._transactionCustomerInfoContainer.style.display = "none";
        this._transactionAllVehicleInfoContainer.style.display = "none";
    }

    handleSelectCustomerBtnClick() {
        const tradeInTab = document.querySelector("#saleTabs button[data-bs-target='#trade-in']");
        bootstrap.Tab.getInstance(tradeInTab).show();
        this.enableTabs();
    }

    enableTabs() {
        this._allTabs.forEach(t => {
            t.removeAttribute("disabled");
            t.removeAttribute("aria-disabled");
        });
    }

    disableTabs() {
        this._allTabs.forEach(t => {
            if (t.getAttribute("id") === "customer-info-tab") return

            t.setAttribute("disabled", true);
            t.setAttribute("aria-disabled", true);
        });
    }

    async fetchTradeInVehicles() {
        try {
            const response = await fetch(`https://localhost:44346/Sale/GetVehiclesOwnedBy?ownerId=${this._customerId}`);
            if (!response.ok) {
                alert("Unable to fetch vehicles for customer - server error, try again later");
            }

            const vehicles = await response.text();
            this._tradeInContainer.innerHTML = vehicles;
        }
        catch {
            alert("Network error - try again later");
        }
        finally {
            const selectElement = document.querySelector(".trade-in-select");
            if (selectElement) {
                selectElement.addEventListener("change", async e => {
                    this.fetchVehicleDetailsFor(e.target.value, this._selectedTradeInDetails);
                    this._tradeInId = e.target.value;
                })
            }
        }
    }

    async fetchAvailableVehicles() {
        try {
            const response = await fetch("https://localhost:44346/Sale/GetAvailableVehicles");
            if (!response.ok) {
                throw new Exception();
            }

            const vehicles = await response.text();
            this._availableVehiclesList.innerHTML = vehicles;
        }
        catch {
            alert("Network error - try again later");
        }
        finally {
            const selectElement = document.querySelector(".available-vehicles__select");
            if (selectElement) {
                selectElement.addEventListener("change", async e => {
                    this.fetchVehicleDetailsFor(e.target.value, this._selectedVehicleDetails);
                    this._purchasedVehicleId = e.target.value;
                });
            }
        }
    }

    async fetchVehicleDetailsFor(id, parentElement) {
        try {
            const response = await fetch(`https://localhost:44346/Sale/GetVehicleDetailsFor/${id}`);
            if (!response.ok) {
                throw new Exception();
            }

            const vehicleInfo = await response.text();
            parentElement.innerHTML = vehicleInfo;
        }
        catch {
            alert("Unable to retrieve vehicle details - try again later");
        }
    }

    async handleTransactionTabClick() {
        if (this.isPurchaseInfoMissing()) return;

        this.showPurchaseForms();
        await this.fetchDataForTransaction();
        this.populateFormWithCustomerData();
        this.populateFormWithTradeInData();
        this.populateFormWithPurchaseData();
    }

    isPurchaseInfoMissing() {
        if ((!this._customerId || !this._purchasedVehicleId) && document.querySelector("#missing-purchased-vehicle-warning")) return true;
        else if (!this._customerId || !this._purchasedVehicleId) {
            const paragraph = document.createElement("p");
            paragraph.textContent = "Please select a vehicle to purchase before proceeding.";
            paragraph.setAttribute("id", "missing-purchased-vehicle-warning");
            this._transactionContainer.append(paragraph);
            this._transactionCustomerInfoContainer.style.display = "none";
            this._transactionAllVehicleInfoContainer.style.display = "none";
            this._transactionWarning = paragraph;
            return true;
        }
        return false;
    }

    showPurchaseForms() {
        if (this._transactionWarning) this._transactionWarning.remove();
        this._transactionCustomerInfoContainer.style.display = "grid";
        this._transactionAllVehicleInfoContainer.style.display = "grid";
    }

    async fetchDataForTransaction() {
        try {
            const tradeInVehiclePromise = this._tradeInId ? fetch(`https://localhost:44346/api/inventory/${this._tradeInId}`).then(res => res.json()) : Promise.resolve();
            const purchasedVehiclePromise = fetch(`https://localhost:44346/api/inventory/${this._purchasedVehicleId}`).then(res => res.json());
            const customerPromise = fetch(`https://localhost:44346/api/user/${this._customerId}`).then(res => res.json());

            const [tradeInVehicle, purchasedVehicle, customer] = await Promise.all([tradeInVehiclePromise, purchasedVehiclePromise, customerPromise]);

            this._customer = customer;
            this._tradeIn = tradeInVehicle;
            this._purchased = purchasedVehicle;
        }
        catch {
            alert("Unable to process transaction - try again later");
        }
    }

    populateFormWithCustomerData() {
        this._transactionName.textContent = `${this._customer.FirstName} ${this._customer.LastName}`;
        this._transactionEmail.textContent = this._customer.Email;
        this._transactionPhone.textContent = this._customer.PhoneNumber ?? "N/A";
        this._transactionAddress1.textContent = this._customer.Address1;
        this._transactionAddress2.textContent = this._customer.Address2;
        this._transactionCity.textContent = this._customer.City;
        this._transactionState.textContent = this._customer.State;
        this._transactionZip.textContent = this._customer.ZipCode;
    }

    populateFormWithTradeInData() {
        if (!this._tradeInId) {
            this._transactionTradeInMakeModel.textContent = "N/A";
            this._transactionTradeInVin.textContent = "N/A";
            this._transactionTradeInValue.textContent = "N/A";
        }
        else {
            this._transactionTradeInMakeModel.textContent = `${this._tradeIn.Model.Make.Name} ${this._tradeIn.Model.Name}`;
            this._transactionTradeInVin.textContent = this._tradeIn.VIN;
            this._transactionTradeInValue.textContent = `$${this._tradeIn.SalePrice}`;
        }
    }

    populateFormWithPurchaseData() {
        this._transactionPurchaseMakeModel.textContent = `${this._purchased.Model.Make.Name} ${this._purchased.Model.Name}`;
        this._transactionPurchaseVin.textContent = this._purchased.VIN;
        this._transactionPurchaseSalePrice.textContent = `$${this._purchased.SalePrice}`;
        this._transactionPurchaseBalanceAfterTradeIn.textContent = this._tradeInId ? `$${(this._purchased.SalePrice - this._tradeIn.SalePrice).toFixed(2)}` : `$${this._purchased.SalePrice}`;
    }

    handlePurchaseMethodSelection(e) {       
        switch (e.target.value) {
            case "cash":
                this.show(this._cashPane);
                return;
            case "bank finance":
                this.show(this._bankPane);
                return;
            case "dealer finance":
                this.show(this._dealerPane);
                return;
            default:
                throw new Exception("Invalid purchase method selected.");
        }
    }

    show(pane) {
        [this._cashPane, this._bankPane, this._dealerPane].forEach(p => p.classList.remove("show"));
        pane.classList.add("show");
    }

    async handleCashSaleClick() {
        const tradeInValue = this._tradeIn?.SalePrice ? this._tradeIn.SalePrice : 0;

        const body = new FormData();
        body.append("VehicleId", this._purchased.Id);
        body.append("TradeInId", this._tradeInId);
        body.append("PurchasePrice", this._purchased.SalePrice);
        body.append("CustomerId", this._customerId);
        body.append("DownPayment", parseFloat(this._cashPurchaseAmount.value, 2) + tradeInValue);
        body.append("PurchaseTypeId", 1);

        await this.handleSale(body);
    }

    async handleDealerSaleClick() {        
        const tradeInValue = this._tradeIn?.SalePrice ? this._tradeIn.SalePrice : 0;

        const body = new FormData();
        body.append("VehicleId", this._purchasedVehicleId);
        body.append("TradeInId", this._tradeInId);
        body.append("PurchasePrice", this._purchased.SalePrice);
        body.append("CustomerId", this._customerId);
        body.append("LoanLength", this._dealerLoanLength.value);
        body.append("InterestRate", parseFloat(this._dealerInterestRate.value / 100, 2));
        body.append("DownPayment", parseFloat(this._dealerDownpayment.value, 2) + tradeInValue);
        body.append("PurchaseTypeId", 3);

        await this.handleSale(body);
    }

    async handleBankSaleClick() {
        const tradeInValue = this._tradeIn?.SalePrice ? this._tradeIn.SalePrice : 0;

        const body = new FormData();
        body.append("VehicleId", this._purchasedVehicleId);
        body.append("Vehicle", JSON.stringify(this._purchased));
        body.append("TradeInId", this._tradeInId);
        body.append("TradeIn", JSON.stringify(this._tradeIn));
        body.append("PurchasePrice", this._purchased.SalePrice);
        body.append("CustomerId", this._customerId);
        body.append("LoanLength", this._bankLoanLength.value);
        body.append("InterestRate", parseFloat(this._bankInterestRate.value / 100, 2));
        body.append("ApprovalAmount", this._bankApprovalAmount.value);
        body.append("ApprovalLetter", this._bankApprovalLetter.files[0]);
        body.append("DownPayment", parseFloat(this._bankDownpayment.value, 2) + tradeInValue);
        body.append("PurchaseTypeId", 2);

        await this.handleSale(body);
    }

    async handleSale(body) {
        try {
            const url = "https://localhost:44346/Sale/ProcessSale"
            const response = await fetch(url, {
                method: "POST",
                headers: {
                    "Accept": "application/pdf"
                },
                body
            });

            if (response.redirected) throw new Exception();

            const data = await response.arrayBuffer();
            const file = new Blob([data], { type: "application/pdf" });
            const fileUrl = window.URL.createObjectURL(file);
            window.open(fileUrl);
            window.location.href = "https://localhost:44346/Sale/Success";
        }
        catch {
            window.location.href = "https://localhost:44346/Sale/Failure";
        }
    }
}

const workflow = new Workflow();