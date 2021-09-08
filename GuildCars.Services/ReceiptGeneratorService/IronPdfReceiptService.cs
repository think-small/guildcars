using GuildCars.Data;
using GuildCars.Models;
using GuildCars.Services.SaleProcessorService;
using IronPdf;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GuildCars.Services.ReceiptGeneratorService
{
    internal class IronPdfReceiptService : IReceiptGeneratorService
    {
        private readonly string bootstrapCdn = @"https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css";
        private readonly HtmlToPdf _renderer;
        private IVehicleSale _purchaseInfo;           
        private string _html = "";
        private string _purchaseType;

        public IronPdfReceiptService()
        {
            _renderer = new HtmlToPdf();
        }
        public string AddApprovalLetter()
        {
            return "";
        }

        public void AddPurchaseInformation()
        {
            _html += $@"
                        <div class='mb-3 container'>
                            <div class='row'>
                                <div class='col-sm-12'><h5>Purchase Information</h5></div>
                            </div>
                            <div class='row'>
                                <div class='col-sm-6'>
                                    <div>Vehicle Purchased:  {_purchaseInfo.Vehicle.Model.Make.Name} {_purchaseInfo.Vehicle.Model.Name} ({_purchaseInfo.Vehicle.Year})</div>
                                    <div>Mileage: {_purchaseInfo.Vehicle.Mileage} miles</div>
                                    <div>VIN: {_purchaseInfo.Vehicle.VIN}</div>
                                    <div>Sale Price: {_purchaseInfo.Vehicle.SalePrice.ToString("C")}</div>
                                </div>
                                <div class='col-sm-6'>
                                    <div>Payment Method: {_purchaseType}</div>
                                    <div>Trade In: {(_purchaseInfo.TradeIn != null ? _purchaseInfo.TradeIn.SalePrice.ToString("C") : "None")}</div>
                                    <div>Total Upfront Payment: {(_purchaseInfo.DownPayment  == 0M ? "None" : _purchaseInfo.DownPayment.ToString("C"))}</div>
                                    <div>Balance: {(_purchaseInfo.Vehicle.SalePrice - _purchaseInfo.DownPayment).ToString("C")}</div>
                                </div>
                            </div>
                        </div>
                        <hr>
                    ";
        }

        public void Configure(IVehicleSale purchaseInfo)
        {
            _purchaseInfo = purchaseInfo;
            _purchaseType = purchaseInfo.ToString();
            _renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Screen;
            _renderer.PrintOptions.CustomCssUrl = bootstrapCdn;
            _renderer.PrintOptions.FirstPageNumber = 1;           
        }

        public void AddAmortizedSchedule(AmortizedLoanSchedule loanSchedule)
        {
            var cumulativeInterestPaid = loanSchedule.Schedule.Max(m => m.CumulativeInterest);
            var originalBalance = loanSchedule.Schedule.Max(m => m.Balance) + loanSchedule.MonthlyPayment;
            string loanInfo = $@"
                                <div class='mb-3 container'>
                                    <div class='row'>
                                        <div class='col-sm-3'>
                                            <div>Origination: {loanSchedule.OriginationDate.Date.ToString("MMM yyyy")}</div>
                                            <div>Closing: {loanSchedule.EndDate.Date.ToString("MMM yyyy")}</div>                                                                                        
                                        </div>
                                        <div class='col-sm-4'>
                                            <div>Monthly Payment: {loanSchedule.MonthlyPayment.ToString("C")}</div>
                                            <div>Interest Rate: {(loanSchedule.InterestRate * 100).ToString("N2")}%</div>
                                        </div>
                                        <div class='col-sm-4'>
                                            <div>Cumulative Interest: {cumulativeInterestPaid.ToString("C")}</div>
                                            <div>Total Due: {(cumulativeInterestPaid + originalBalance).ToString("C")}</div>
                                        </div>
                                    </div>
                                </div>
                              ";
            string table = @"
                            <div class='mb-3 container'>
                                <div class='row'>
                                    <div class='col-sm-12'>
                                        <table class='table table-hover table-striped'>
                                            <thead>
                                                <tr>
                                                    <th scope='col'>Number</th>
                                                    <th scope='col'>Due Date</th>
                                                    <th scope='col'>Payment Amount</th>
                                                    <th scope='col'>Principal</th>
                                                    <th scope='col'>Interest</th>
                                                    <th scope='col'>Cumulative Interest Paid</th>
                                                    <th scope='col'>Balance</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                           ";
            int counter = 1;
            foreach (var payment in loanSchedule.Schedule)
            {
                table += $@"
                            <tr>
                                <td>{counter}</td>
                                <td>{payment.DueDate.Date.ToString("MMM yyyy")}</td>
                                <td>{payment.PaymentAmount}</td>
                                <td>{payment.Principal}</td>
                                <td>{payment.Interest}</td>
                                <td>{payment.CumulativeInterest}</td>
                                <td>{payment.Balance}</td>
                            </tr>
                         ";
                counter++;
            }
            table += "</tbody></table></div></div></div>";

            loanInfo += table;
            _html += loanInfo;
        }

        public void CreateFooter()
        {
            _html += $@"
                        <div class='container mt-5'>
                            <div class='row'>
                                <div class='col-sm-6'>
                                    <div style='min-width: 50%; border-bottom: 2px solid black;'></div>
                                    <small style='display: block; margin-bottom: 3em;'>Buyer's Signature</small>

                                    <div style='min-width: 50%; border-bottom: 2px solid black;'></div>
                                    <small>Date</small>
                                </div>
                                <div class='col-sm-6'>
                                    <div style='min-width: 50%; border-bottom: 2px solid black;'></div>
                                    <small style='display: block; margin-bottom: 3em;'>Sales Employee's Signature</small>

                                    <div style='min-width: 50%; border-bottom: 2px solid black;'></div>
                                    <small>Date</small>
                                </div>
                            </div>
                        </div>
                      ";
        }

        public void CreateHeader()
        {
            _html += $@"
                <header>
                    <div class='mb-3 container'>
                        <div class='row'>
                            <div class='col-sm-6'>
                                <h2>Guild Cars</h2>
                                <h3>Purchase Agreement</h3>
                                <div>{DateTime.Today.Date.ToString("ddd, dd MMM yyy")}</div>
                            </div>
                            <div class='col-sm-6'>
                                <address>
                                    <div>Guild Cars LLC</div>
                                    <div>12345 Cars For Sale Lane</div>
                                    <div>Minneapolis, MN 55318</div>
                                </address>
                            </div>
                        </div>
                    </div>
                </header>
                <hr>
            ";
        }

        public Stream GetPdf()
        {
            var pdf = _renderer.RenderHtmlAsPdf(_html);
            var outputPath = $"{_purchaseInfo.Vehicle.Model.Make.Name}-{_purchaseInfo.Vehicle.Model.Name}-{_purchaseInfo.Vehicle.Year}-{_purchaseInfo.CustomerId}.pdf";
            //pdf.SaveAs(outputPath);

            //  System.Diagnostics.Process.Start(outputPath);  For Debugging Purposes Only

            return pdf.Stream;
        }
    }
}
