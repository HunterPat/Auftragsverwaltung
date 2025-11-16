
using Backend.Dtos;
using Backkend.Dtos;
using iText.Html2pdf;
using iText.Layout.Font;
using iText.Svg.Renderers.Impl;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Backend.Services
{
    public class MyService
    {
        public MySqlConnection dbConnection;
        public readonly string filePath = $"{Path.GetDirectoryName(Directory.GetCurrentDirectory())}/Backend/Files/Logs.txt";
        private IConfiguration _config;
        public MyService(IConfiguration configuration)
        {
            _config = configuration;
            dbConnection = new MySqlConnection("Server=localhost;Database=order_management;User ID=root;Password=EaPatrice445A.a;");
            try
            {
                dbConnection.Open();
            }
            catch (MySqlException ex)
            {
                File.AppendAllText(filePath, $"{DateTime.Now:dd.MM.yyyy, HH:mm:ss} | SQL-Exception (MyService|MyService): {ex.Message}\n");
            }
        }
        //################ Customers #####################
        public List<CustomerDto> GetAllCustomers()
        {
            var list = new List<CustomerDto>();
            try
            {

                MySqlCommand categoryCommand = new("SELECT * FROM Customers", dbConnection);
                using (var reader = categoryCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new CustomerDto { Id = int.Parse(reader["CustomerId"].ToString()!), Name = reader["Name"].ToString()! });
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            return list;
        }
        public ResponseDto AddCustomer(CustomerDto newCustomer)
        {
            try
            {
                if (newCustomer == null) return new ResponseDto { Message = "Customer is null", Status = false };
                MySqlCommand categoryInsertCommand = new("INSERT INTO Customers (Name) VALUES (@name)", dbConnection);
                categoryInsertCommand.Parameters.AddWithValue("@name", newCustomer.Name);
                if (categoryInsertCommand.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine("Customer inserted successfully.");
                }
                else Console.WriteLine("Failed to insert Customer!");

                return new ResponseDto { Status = true, Message = "Okay" };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    Status = false,
                    Message = ex.Message
                };
            }

        }
        public ResponseDto UpdateCustomer(CustomerDto newCustomer)
        {
            throw new NotImplementedException();
        }

        public ResponseDto DeleteCustomer(int customerId)
        {
            try
            {
                if (customerId < 0) return new ResponseDto { Message = "customerId need to be greater than -1", Status = false };
                MySqlCommand categoryDeleteCommand = new("DELETE FROM Customers WHERE CustomerId = @id", dbConnection);
                categoryDeleteCommand.Parameters.AddWithValue("@id", customerId);

                if (categoryDeleteCommand.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine("Customer deletion successfully.");
                    return new ResponseDto { Status = true, Message = "Okay" };

                }
                else Console.WriteLine("Failed to delete Customer!");
                return new ResponseDto { Status = false, Message = $"No customer with Id {customerId} found!" };


            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        //################ ORDERS #####################
        public List<OrderDto> GetAllOrders()
        {
            var list = new List<OrderDto>();
            try
            {

                MySqlCommand ordersCommand = new MySqlCommand(
                    "SELECT o.*, c.Name AS CustomerName, s.Name AS StatusName " +
                    "FROM `Orders` o " +
                    "JOIN Customers c ON c.CustomerId = o.FK_CustomerId " +
                    "JOIN Status s ON s.StatusId = o.FK_StatusId",
                    dbConnection);
                using (var reader = ordersCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new OrderDto
                        {
                            Id = int.Parse(reader["OrderId"].ToString()!),
                            InputDate = DateTime.Parse(reader["InputDate"].ToString()!),
                            DeliveryDate = reader["DeliveryDate"]?.ToString()?.Length > 0 ? DateTime.Parse(reader["DeliveryDate"]?.ToString()!) : null!,
                            PaymentDate = reader["PaymentDate"]?.ToString()?.Length > 0 ? DateTime.Parse(reader["PaymentDate"]?.ToString()!) : null!,
                            DocumentNr = int.Parse(reader["DocumentNr"].ToString()!),
                            Brutto = double.Parse(reader["Brutto"].ToString()!),
                            Netto = double.Parse(reader["Netto"].ToString()!),
                            Tax = double.Parse(reader["Tax"].ToString()!),
                            CustomerName = reader["CustomerName"].ToString()!,
                            Bill = int.Parse(reader["Bill"].ToString()!),
                            PO = reader["PO"].ToString()!,
                            Status = reader["StatusName"].ToString()!,
                            UID = reader["UID"].ToString()!,
                            BillCreatedDate = reader["BillCreatedDate"]?.ToString()?.Length > 0 ? DateTime.Parse(reader["BillCreatedDate"]?.ToString()!) : null!
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            return list;
        }
        public OrderDto GetOrder(string po)
        {
            try
            {
                MySqlCommand ordersCommand = new MySqlCommand(
                    "SELECT o.*, c.Name AS CustomerName, s.Name AS StatusName " +
                    "FROM `Orders` o " +
                    "JOIN Customers c ON c.CustomerId = o.FK_CustomerId " +
                    "JOIN Status s ON s.StatusId = o.FK_StatusId WHERE o.po = @po",
                    dbConnection);
                ordersCommand.Parameters.AddWithValue("@po", po);
                using (var reader = ordersCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new OrderDto
                        {
                            Id = int.Parse(reader["OrderId"].ToString()!),
                            InputDate = DateTime.Parse(reader["InputDate"].ToString()!),
                            DeliveryDate = reader["DeliveryDate"]?.ToString()?.Length > 0 ? DateTime.Parse(reader["DeliveryDate"]?.ToString()!) : null!,
                            PaymentDate = reader["PaymentDate"]?.ToString()?.Length > 0 ? DateTime.Parse(reader["PaymentDate"]?.ToString()!) : null!,
                            DocumentNr = int.Parse(reader["DocumentNr"].ToString()!),
                            Brutto = double.Parse(reader["Brutto"].ToString()!),
                            Netto = double.Parse(reader["Netto"].ToString()!),
                            Tax = double.Parse(reader["Tax"].ToString()!),
                            CustomerName = reader["CustomerName"].ToString()!,
                            Bill = int.Parse(reader["Bill"].ToString()!),
                            PO = reader["PO"].ToString()!,
                            Status = reader["StatusName"].ToString()!,
                            UID = reader["UID"].ToString()!,
                            BillCreatedDate = reader["BillCreatedDate"]?.ToString()?.Length > 0 ? DateTime.Parse(reader["BillCreatedDate"]?.ToString()!) : null!
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            return null!;
        }
        public ResponseDto AddOrder(OrderDto newOrder)
        {
            try
            {

                if (CheckIfDocumentNrExsists(newOrder)) return new ResponseDto { Message = $"Order with DocumentNr {newOrder.DocumentNr} already exsists!", Status = false };
                if (newOrder == null) return new ResponseDto { Message = "Object is null", Status = false };
                if (newOrder.DeliveryDate != null || newOrder.PaymentDate != null)
                    if (newOrder.InputDate > newOrder.DeliveryDate || newOrder.InputDate > newOrder.PaymentDate) return new ResponseDto { Message = "Eingangsdatum ist größer als Lieferdatum oder Zahlungsdatum", Status = false };
                var orderIn = CheckIfForeignkeysExsists(newOrder);

                if (orderIn == null) return new ResponseDto { Message = "Customer or Status not found!", Status = false };
                MySqlCommand orderInsertCommand = new MySqlCommand(
                    "INSERT INTO `Orders` (PO, UID, PaymentDate, Bill, Tax, Brutto, Netto, DeliveryDate, DocumentNr, FK_CustomerId, FK_StatusId, InputDate, BillCreatedDate) " +
                    "VALUES (@PO, @UID, @PaymentDate, @Bill, @Tax, @Brutto, @Netto, @DeliveryDate, @DocumentNr, @FK_CustomerId, @FK_StatusId, @InputDate, @BillCreatedDate)",
                    dbConnection); orderInsertCommand.Parameters.AddWithValue("@PO", orderIn.PO);
                orderInsertCommand.Parameters.AddWithValue("@UID", orderIn.UID);
                orderInsertCommand.Parameters.AddWithValue("@PaymentDate", orderIn.PaymentDate);
                orderInsertCommand.Parameters.AddWithValue("@Bill", orderIn.Bill);
                orderInsertCommand.Parameters.AddWithValue("@Tax", orderIn.Tax);
                orderInsertCommand.Parameters.AddWithValue("@Brutto", orderIn.Brutto);
                orderInsertCommand.Parameters.AddWithValue("@Netto", orderIn.Netto);
                orderInsertCommand.Parameters.AddWithValue("@DeliveryDate", orderIn.DeliveryDate);
                orderInsertCommand.Parameters.AddWithValue("@DocumentNr", orderIn.DocumentNr);
                orderInsertCommand.Parameters.AddWithValue("@FK_CustomerId", orderIn.FK_CustomerId);
                orderInsertCommand.Parameters.AddWithValue("@FK_StatusId", orderIn.FK_StatusId);
                orderInsertCommand.Parameters.AddWithValue("@InputDate", orderIn.InputDate);
                orderInsertCommand.Parameters.AddWithValue("@@BillCreatedDate", orderIn.BillCreatedDate);

                if (orderInsertCommand.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine("Order inserted successfully.");
                }
                else Console.WriteLine("Failed to insert Order!");


                return new ResponseDto { Status = true, Message = "Okay" };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        private bool CheckIfDocumentNrExsists(OrderDto newOrder)
        {
            var orderCheckCommand = new MySqlCommand("Select * FROM Orders Where documentNr = @documentNr", dbConnection);
            orderCheckCommand.Parameters.AddWithValue("@documentNr", newOrder.DocumentNr);
            var reader = orderCheckCommand.ExecuteReader();
            var hasRows = reader.HasRows;
            reader.Close();
            if (hasRows)
            {
                return true;
            }
            else
                return false;
        }

        private OrderDtoIn CheckIfForeignkeysExsists(OrderDto newOrder)
        {
            try
            {
                var orderIn = new OrderDtoIn { }.CopyFrom(newOrder);
                MySqlCommand checkCustomerExsistsCommand = new("Select * from Customers WHERE Name = @customerName", dbConnection);
                MySqlCommand checkStatusExsistsCommand = new("Select * from Status WHERE Name = @status", dbConnection);
                checkCustomerExsistsCommand.Parameters.AddWithValue("@customerName", newOrder.CustomerName);
                checkStatusExsistsCommand.Parameters.AddWithValue("@status", newOrder.Status);
                var statusReader = checkStatusExsistsCommand.ExecuteReader();
                if (statusReader.HasRows)
                {
                    statusReader.Read();
                    orderIn.FK_StatusId = int.Parse(statusReader["StatusId"].ToString()!);
                    statusReader.Close();

                }
                else
                {
                    statusReader.Close();
                    return null!;
                }
                var customerReader = checkCustomerExsistsCommand.ExecuteReader();
                if (customerReader.HasRows)
                {
                    customerReader.Read();
                    orderIn.FK_CustomerId = int.Parse(customerReader["CustomerId"].ToString()!);
                    customerReader.Close();
                }
                else
                {
                    customerReader.Close();
                    AddCustomer(new CustomerDto { Name = newOrder.CustomerName });
                    customerReader = checkCustomerExsistsCommand.ExecuteReader();
                    if (customerReader.HasRows)
                    {
                        customerReader.Read();
                        orderIn.FK_CustomerId = int.Parse(customerReader["CustomerId"].ToString()!);
                        customerReader.Close();
                    }
                }
                return orderIn;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            return null!;

        }
        public ResponseDto UpdateOrder(int oldOrderId, OrderDto updatedOrder)
        {
            try
            {
                if (updatedOrder == null)
                    return new ResponseDto { Message = "Object is null", Status = false };

                MySqlCommand checkOrder = new MySqlCommand("SELECT COUNT(*) FROM `Orders` WHERE OrderId = @Id", dbConnection);
                checkOrder.Parameters.AddWithValue("@Id", oldOrderId);

                int orderExists = checkOrder.ExecuteNonQuery();
                if (orderExists == 0)
                    return new ResponseDto { Message = $"No order with Id {oldOrderId} found!", Status = false };

                MySqlCommand orderUpdateCommand = new MySqlCommand(
                    "UPDATE `Orders` SET " +
                    "PO = @PO, UID = @UID, PaymentDate = @PaymentDate, Bill = @Bill, Tax = @Tax, Brutto = @Brutto, Netto = @Netto, " +
                    "DeliveryDate = @DeliveryDate, DocumentNr = @DocumentNr, FK_CustomerId = (Select CustomerId FROM Customers Where Name = @CustomerName), FK_StatusId = (Select StatusId FROM Status Where Name = @StatusName), " +
                    "InputDate = @InputDate, " +
                    "BillCreatedDate = @BillCreatedDate " +
                    "WHERE OrderId = @Id", dbConnection);

                orderUpdateCommand.Parameters.AddWithValue("@PO", updatedOrder.PO);
                orderUpdateCommand.Parameters.AddWithValue("@UID", updatedOrder.UID);
                orderUpdateCommand.Parameters.AddWithValue("@PaymentDate", updatedOrder.PaymentDate);
                orderUpdateCommand.Parameters.AddWithValue("@Bill", updatedOrder.Bill);
                orderUpdateCommand.Parameters.AddWithValue("@Tax", updatedOrder.Tax);
                orderUpdateCommand.Parameters.AddWithValue("@Brutto", updatedOrder.Brutto);
                orderUpdateCommand.Parameters.AddWithValue("@Netto", updatedOrder.Netto);
                orderUpdateCommand.Parameters.AddWithValue("@DeliveryDate", updatedOrder.DeliveryDate);
                orderUpdateCommand.Parameters.AddWithValue("@DocumentNr", updatedOrder.DocumentNr);
                orderUpdateCommand.Parameters.AddWithValue("@CustomerName", updatedOrder.CustomerName);
                orderUpdateCommand.Parameters.AddWithValue("@StatusName", updatedOrder.Status);
                orderUpdateCommand.Parameters.AddWithValue("@InputDate", updatedOrder.InputDate);
                orderUpdateCommand.Parameters.AddWithValue("@Id", oldOrderId);
                orderUpdateCommand.Parameters.AddWithValue("@BillCreatedDate", updatedOrder.BillCreatedDate);

                int rowsAffected = orderUpdateCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Order updated successfully.");
                    return new ResponseDto { Status = true, Message = "Order updated successfully." };
                }
                else
                {
                    Console.WriteLine("Failed to update order.");
                    return new ResponseDto { Status = false, Message = "Failed to update order." };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }
        //################ Status #####################

        public List<StatusDto> GetAllStatus()
        {
            var list = new List<StatusDto>();
            try
            {

                MySqlCommand categoryCommand = new("SELECT * FROM Status", dbConnection);
                using (var reader = categoryCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new StatusDto { Id = int.Parse(reader["StatusId"].ToString()!), Name = reader["Name"].ToString()! });
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            return list;
        }
        public ResponseDto AddStatus(StatusDto newStatus)
        {
            try
            {
                if (newStatus == null) return new ResponseDto { Message = "Status is null", Status = false };
                MySqlCommand statusInsertCommand = new("INSERT INTO Status (Name) VALUES (@name)", dbConnection);
                statusInsertCommand.Parameters.AddWithValue("@name", newStatus.Name);
                if (statusInsertCommand.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine("Status inserted successfully.");
                }
                else Console.WriteLine("Failed to insert Status!");

                return new ResponseDto { Status = true, Message = "Okay" };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    Status = false,
                    Message = ex.Message
                };
            }

        }
        //######## ORDERS ##########
        public YearInfoDto GetYearInfo(string year)
        {
            var yearInfo = new YearInfoDto();
            try
            {
                for (int i = 1; i <= 4; i++)
                {
                    MySqlCommand ordersCommand = new MySqlCommand();
                    switch (i)
                    {
                        case 1:
                            ordersCommand = new( //In Arbeit
                               "SELECT SUM(o.brutto) as SumBrutto, SUM(o.Netto) as SumNetto, o.FK_StatusId " +
                               "FROM `Orders` o " +
                               "Where o.FK_StatusId = @statusId AND o.InputDate <= @dateHigh AND o.InputDate >= @dateLow " +
                               "GROUP BY o.FK_StatusId"
                               , dbConnection);
                            break;
                        case 2:
                            ordersCommand = new( //Erledigt
                               "SELECT SUM(o.brutto) as SumBrutto, SUM(o.Netto) as SumNetto, FK_StatusId " +
                               "FROM `Orders` o " +
                               "Where o.FK_StatusId = @statusId AND o.DeliveryDate <= @dateHigh AND o.DeliveryDate >= @dateLow " +
                               "GROUP BY o.FK_StatusId"
                               , dbConnection);
                            break;
                        case 3:
                            ordersCommand = new( // Verrechnet
                               "SELECT SUM(o.brutto) as SumBrutto, SUM(o.Netto) as SumNetto, FK_StatusId " +
                               "FROM `Orders` o " +
                               "Where o.FK_StatusId = @statusId AND o.DeliveryDate <= @dateHigh AND o.DeliveryDate >= @dateLow " +
                               "GROUP BY o.FK_StatusId"
                               , dbConnection);
                            break;
                        case 4:
                            ordersCommand = new( // Bezahlt
                               "SELECT SUM(o.brutto) as SumBrutto, SUM(o.Netto) as SumNetto, FK_StatusId " +
                               "FROM `Orders` o " +
                               "Where o.FK_StatusId = @statusId AND o.PaymentDate <= @dateHigh AND o.PaymentDate >= @dateLow " +
                               "GROUP BY o.FK_StatusId"
                               , dbConnection);
                            break;
                    }
                    ordersCommand.Parameters.AddWithValue("@dateHigh", $"{year}-12-31 00:00:00");
                    ordersCommand.Parameters.AddWithValue("@dateLow", $"{year}-01-01 00:00:00");
                    ordersCommand.Parameters.AddWithValue("@statusId", i);
                    using (var reader = ordersCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            switch (reader["FK_StatusId"].ToString())
                            {
                                case "1": yearInfo.InArbeitBrutto = double.Parse(reader["SumBrutto"].ToString()!); yearInfo.InArbeitNetto = double.Parse(reader["SumNetto"].ToString()!); break;
                                case "2": yearInfo.ErledigtBrutto = double.Parse(reader["SumBrutto"].ToString()!); yearInfo.ErledigtNetto = double.Parse(reader["SumNetto"].ToString()!); break;
                                case "3": yearInfo.VerrechnetBrutto = double.Parse(reader["SumBrutto"].ToString()!); yearInfo.VerrechnetNetto = double.Parse(reader["SumNetto"].ToString()!); break;
                                case "4": yearInfo.BezahltBrutto = double.Parse(reader["SumBrutto"].ToString()!); yearInfo.BezahltNetto = double.Parse(reader["SumNetto"].ToString()!); break;

                            }
                        }
                    }
                }
                yearInfo.SumBrutto = yearInfo.InArbeitBrutto + yearInfo.ErledigtBrutto + yearInfo.VerrechnetBrutto + yearInfo.BezahltBrutto;
                yearInfo.SumNetto = yearInfo.InArbeitNetto + yearInfo.ErledigtNetto + yearInfo.VerrechnetNetto + yearInfo.BezahltNetto;
                yearInfo.SumTax = yearInfo.SumBrutto - yearInfo.SumNetto;


                return yearInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            return null!;
        }

        public ResponseDto UpdateStatusOfOrders(List<OrderDto> orders)
        {
            try
            {
                orders.ForEach(x => UpdateOrder(x.Id, x));
                return new ResponseDto { Message = "OK", Status = true };
            }
            catch (Exception ex)
            {
                File.AppendAllText(filePath, $"{DateTime.Now:dd.MM.yyyy, HH:mm:ss} | (MyService|UpdateStatusOfOrders): {ex.Message}\n");
                return new ResponseDto { Message = $"{ex.Message}", Status = false };
            }
        }

        public ResponseDto CreatePdfForOrders(List<string> orders)
        {
            try
            {
                var invoiceNumber = 0;
                var rate = 0.0;
                var foundOrders = new List<OrderDto>();
                foreach (var orderLine in orders)
                {
                    if (orderLine.Contains(','))
                    {
                        var foundOrder = GetOrder(orderLine.Split(",")[0]);
                        if (foundOrder != null)
                        {
                            rate += foundOrder.Brutto;
                            if (foundOrder.DocumentNr > invoiceNumber)
                                invoiceNumber = foundOrder.DocumentNr;
                            foundOrders.Add(foundOrder);

                        }
                    }
                }
                string htmlContent = @"
                    <!DOCTYPE html>
                    <html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""en"" lang=""en"">
                    <head>
                        <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""/>
                        <title>Rechnungsvorlage-Global Lingo</title>
                        <meta name=""author"" content=""Marcel Keiser""/>
                        <style type=""text/css"">
                            * { margin: 0; padding: 0; text-indent: 0; font-family:Arial;}
                            h1 { color: black; font-family: Arial, Helvetica; font-size: 16pt; font-weight: bold; }
                            h2 { color: black; font-family: Arial, Helvetica;  font-size: 12pt; font-weight: bold; }
                            p { color: black; font-family: Arial, Helvetica;  font-size: 12pt; }
                            .s1 { color: #808080; font-family: Arial, Helvetica;  font-size: 10pt; font-style: italic; }
                            .s2 { font-size: 12pt; }
                            .s3 { font-size: 12pt; font-weight: bold; }
                            .s4 { font-size: 12pt; font-weight: bold; }
                            .s5 { font-size: 10pt; }
                            .s6, .s7, .s8 { font-size: 8pt; }
                            .invoiceTable { vertical-align: top; border-collapse: collapse; border:1px solid black}
                        </style>
                    </head>
                    <body style=""border: 20px solid #F2F2F2; padding: 10px; font-family: Arial, Helvetica"">
                        <h1 style=""text-align: right;"">INVOICE</h1>
                        <div style=""margin-bottom: 30px"">
                        <img src=""img/logo.jpg"" style=""width:180px; height:100px"">
                        <p class=""s1"" style="""">Marcel Keiser</p>
                        <p class=""s1"" style="" line-height: 133%;"">Sonnenhang 12, 4725 St. Aegidi</p>
                        <p class=""s1"" style="" line-height: 133%;"">Austria</p>
                        </div>
                        <div style=""display: flex; justify-content: space-between;"">
                                <div>
                                    <h2>Recipient: <br>Global Lingo Ltd</h2>
                                    <br>
                                    <p>Edinburgh House – Unit KL123</p>
                                    <p>170 Kennington Lane</p> 
                                    <p>London, SE11 5DP</p>
                                    <p>United Kingdom</p>
                                    <p>UID: GB877714869</p>
                                </div>                           
                                <div style=""background:#F2F2F2; width:300px; padding:20px;margin-left:90px; text-align: center; vertical-align: middle;"">
                                    <p class=""s2"">Invoice date: " + DateTime.Now.ToString("yyyy-MM-dd") + @"</p>
                                    <p class=""s2"">Invoice number: " + invoiceNumber + @"</p>
                                    <br>
                                    <p class=""s2"">PO number: see table</p>
                                    <p class=""s2"">Payment period: 45 days</p>
                                    <p class=""s2"">Due date: " + DateTime.Now.AddDays(45).ToString("yyyy-MM-dd") + @"</p>
                            </div>
                        </div>
                        <h2 style=""margin: 30px 0 30px 0"">Additional information</h2>
                        
                        <table class=""invoiceTable"">
                            <thead>
                            <tr class=""invoiceTable"" bgcolor=""#D6DBE4"">
                                <th class=""invoiceTable"" style=""width:300px; text-align:left;"">Task unit</th>
                                <th class=""invoiceTable"" style=""width:80px;text-align:right;"">Quantity</th>
                                <th class=""invoiceTable"" style=""width:80px;text-align:right;"">Rate</th>
                                <th class=""invoiceTable"" style=""width:80px;text-align:right;"">Amount</th>
                            </tr>
                            </thead>                            
                            <tbody>
                            <tr class=""invoiceTable"">
                                <td class=""invoiceTable"" style=""text-align:left;"">Multiple POs</td>
                                <td class=""invoiceTable"" style=""text-align: right;height: 200px"">1</td>
                                <td class=""invoiceTable"" style=""text-align: right;height: 200px"">" + Math.Round(rate,2) + @" €</td>
                                <td class=""invoiceTable"" style=""text-align: right;height: 200px"">" + Math.Round(rate,2) + @" €</td>
                            </tr>
                        </tbody>                        
                        </table>
                        <div style=""margin-right: 100px;"">
                        <p class=""s4"" style=""text-align: right; margin-top:30px""><span style=""border-bottom:3px solid black double;"">Invoice amount: " + Math.Round(rate,2) + @" €</span></p>
                        </div>
                        <p class=""s5"" style=""text-align: center; margin: 20px 0 30px 0;"">Please transfer the invoice amount within the payment period.</p>
                        <table style=""border-top:2px solid grey; text-align:left; font-size: 10pt"">
                            <thead>
                            <tr>
                                <th style=""text-align:left;"">Keiser Intelligent<br> Content</th>
                                <th style=""text-align:left;"">Contact</th>
                                <th style=""text-align:left;"" colspan=""2"">Bank account</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr >
                                <td style=""padding:3px"">Sonnenhang 12</td>
                                <td style=""padding:3px"">Marcel Keiser</td>
                                <td style=""padding:3px"">IBAN</td>
                                <td style=""padding:3px"">AT473445500006921589</td>
                            </tr>                   
                            <tr>                    
                                <td style=""padding:3px"">4725 St. Aegidi</td>
                                <td style=""padding:3px"">Phone: +43 660 2126291</td>
                                <td style=""padding:3px"">SWIFT/BIC</td>
                                <td style=""padding:3px"">RZOOAT2L455</td>
                            </tr>                   
                            <tr>                    
                                <td style=""padding:3px"">Austria</td>
                                <td style=""padding:3px"">Email: <a href=""mailto:marcel.keiser69@gmail.com"">marcel.keiser69@gmail.com</a></td>
                                <td style=""padding:3px"">Bank name</td>
                                <td style=""padding:3px"">Raiffeisenbank Region Schärding</td>
                                                    
                            </tr>                   
                            <tr>                    
                                <td style=""padding:3px"">UID ATU72998848</td>
                                <td style=""padding:3px""></td>
                                <td style=""padding:3px"">Address</td>
                                <td style=""padding:3px"">St. Aegidi 61, 4725 St. Aegidi</td>
                            </tr>                   
                            <tr>                    
                                <td style=""padding:3px""></td>
                                <td style=""padding:3px""></td>
                                <td style=""padding:3px"">Account holder</td>
                                <td style=""padding:5px"">Marcel Keiser</td>
                            </tr>
                            </tbody>
                        </table>
                    </body>
                    </html>
                ";

                string destPath = @"D:\Users\Marcel\Documents\KIC\Verwaltung\Rechnungen aus\" + $"{DateTime.Now.ToString("yyyy-MM-dd")}-GL-Invoice-{invoiceNumber}.pdf"; //Use specific path

                using (FileStream pdfStream = new(destPath, FileMode.Create))
                {
                    HtmlConverter.ConvertToPdf(htmlContent, pdfStream);
                }
                Console.WriteLine("PDF created successfully!");
                foreach (var order in foundOrders)
                {
                    order.Bill = invoiceNumber;
                    order.BillCreatedDate = DateTime.Now;
                    order.Status = "Verrechnet";
                    UpdateOrder(order.Id, order);
                }

                return new ResponseDto { Message = "OK", Status = true };
            }
            catch (Exception ex)
            {
                File.AppendAllText(filePath, $"{DateTime.Now:dd.MM.yyyy, HH:mm:ss} | (MyService|CreatePdfForOrders): {ex.Message}\n");
                return new ResponseDto { Message = ex.Message, Status = false };

            }
        }

        public ResponseDto DeleteOrder(List<int> orderIds)
        {
            try
            {
                if (orderIds.Count < 0) return new ResponseDto { Message = "List is empty!", Status = false };
                orderIds.ForEach(orderId =>
                {
                    MySqlCommand categoryDeleteCommand = new("DELETE FROM Orders WHERE OrderId = @id", dbConnection);
                    categoryDeleteCommand.Parameters.AddWithValue("@id", orderId);

                    if (categoryDeleteCommand.ExecuteNonQuery() > 0)
                    {
                        Console.WriteLine("order deletion successfully.");
                    }
                    else Console.WriteLine("Failed to delete order!");
                });
                return new ResponseDto { Status = true, Message = "Okay" };

            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public ResponseDto ImportOrdersFromCsv(List<string> newOrderLines)
        {
            var order = new OrderDto();
            GroupCollection groups = null!;
            try
            {
                for (int i = 0; i < newOrderLines.Count; i++)
                {

                    var regex = new Regex(@"^(?<eingangsdatum>[^;]*);(?<lieferdatum>[^;]*);(?<zahlungsdatum>[^;]*);(?<belegNr>[^;]*);(?<brutto>[^;]*);(?<ust>[^;]*);(?<netto>[^;]*);(?<steuer>[^;]*);(?<kunde>[^;]+);(?<rechnung>[^;]*);(?<po>[^;]+);(?<status>[^;]+);(?<uid>[^;]*)$");
                    groups = regex.Match(newOrderLines[i]).Groups;
                    if (groups.Count >= 3)
                    {
                        if (!string.IsNullOrEmpty(groups["eingangsdatum"].Value.ToString()))
                        {
                            order = new OrderDto
                            {
                                InputDate = DateTime.Parse(groups["eingangsdatum"].Value.ToString()),

                                DeliveryDate = string.IsNullOrEmpty(groups["lieferdatum"].Value)
            ? (DateTime?)null
            : DateTime.Parse(groups["lieferdatum"].Value),

                                PaymentDate = string.IsNullOrEmpty(groups["zahlungsdatum"].Value)
            ? (DateTime?)null
            : DateTime.Parse(groups["zahlungsdatum"].Value),

                                DocumentNr = string.IsNullOrEmpty(groups["belegNr"].Value)
            ? 0
            : int.Parse(groups["belegNr"].Value),

                                Brutto = string.IsNullOrEmpty(groups["brutto"].Value)
            ? 0.0
            : double.Parse(groups["brutto"].Value.Replace("€", "").Replace(",", "").Replace(".", ",")),

                                BillCreatedDate = null!,
                                Tax = string.IsNullOrEmpty(groups["ust"].Value)
            ? 0.0
            : double.Parse(groups["ust"].Value.Replace("%", "")),

                                Netto = string.IsNullOrEmpty(groups["netto"].Value)
            ? 0.0
            : double.Parse(groups["netto"].Value.Replace("€", "").Replace(",", "").Replace(".", ",")),

                                CustomerName = string.IsNullOrEmpty(groups["kunde"].Value)
            ? ""
            : groups["kunde"].Value,

                                Bill = string.IsNullOrEmpty(groups["rechnung"].Value)
            ? 0
            : int.Parse(groups["rechnung"].Value),

                                PO = string.IsNullOrEmpty(groups["po"].Value)
            ? ""
            : groups["po"].Value,

                                Status = string.IsNullOrEmpty(groups["status"].Value)
            ? ""
            : groups["status"].Value,

                                UID = string.IsNullOrEmpty(groups["uid"].Value)
            ? ""
            : groups["uid"].Value.Replace("\r", "")
                            };
                            AddOrder(order);
                        }
                    }
                }
                return new ResponseDto { Message = "Ok", Status = true };
            }
            catch (Exception ex)
            {

                return new ResponseDto { Message = ex.Message, Status = false };
            }
        }

        internal List<MonthInfo> GetMonthsInfos(string year)
        {
            var monthsInfo = new List<MonthInfo>();
            try
            {
                for (int i = 1; i < 13; i++)
                {
                    var monthInfo = new MonthInfo();
                    monthInfo.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(i.ToString("D2")!));
                    for (int j = 1; j <= 4; j++)
                    {
                        MySqlCommand ordersCommand = new MySqlCommand();
                        switch (j)
                        {
                            case 1:
                                ordersCommand = new( //In Arbeit
                                   "SELECT SUM(o.brutto) as SumBrutto, o.FK_StatusId " +
                                   "FROM `Orders` o " +
                                   "Where o.FK_StatusId = @statusId AND o.InputDate <= @dateHigh AND o.InputDate >= @dateLow " +
                                   "GROUP BY o.FK_StatusId"
                                   , dbConnection);
                                break;
                            case 2:
                                ordersCommand = new( //Erledigt
                                   "SELECT SUM(o.brutto) as SumBrutto, FK_StatusId " +
                                   "FROM `Orders` o " +
                                   "Where o.FK_StatusId = @statusId AND o.DeliveryDate <= @dateHigh AND o.DeliveryDate >= @dateLow " +
                                   "GROUP BY o.FK_StatusId"
                                   , dbConnection);
                                break;
                            case 3:
                                ordersCommand = new( // Verrechnet
                                   "SELECT SUM(o.brutto) as SumBrutto, FK_StatusId " +
                                   "FROM `Orders` o " +
                                   "Where o.FK_StatusId = @statusId AND o.DeliveryDate <= @dateHigh AND o.DeliveryDate >= @dateLow " +
                                   "GROUP BY o.FK_StatusId"
                                   , dbConnection);
                                break;
                            case 4:
                                ordersCommand = new( // Bezahlt
                                   "SELECT SUM(o.brutto) as SumBrutto, FK_StatusId " +
                                   "FROM `Orders` o " +
                                   "Where o.FK_StatusId = @statusId AND o.PaymentDate <= @dateHigh AND o.PaymentDate >= @dateLow " +
                                   "GROUP BY o.FK_StatusId"
                                   , dbConnection);
                                break;
                        }
                        var dateHigh = $"{year}-{i.ToString("D2")}-{DateTime.DaysInMonth(int.Parse(year), int.Parse(i.ToString("D2")))}";
                        var dateLow = $"{year}-{i.ToString("D2")}-01";
                        ordersCommand.Parameters.AddWithValue("@dateHigh", $"{year}-{i.ToString("D2")}-{DateTime.DaysInMonth(int.Parse(year), int.Parse(i.ToString("D2")))}");
                        ordersCommand.Parameters.AddWithValue("@dateLow", $"{year}-{i.ToString("D2")}-01");
                        ordersCommand.Parameters.AddWithValue("@statusId", j);
                        using (var reader = ordersCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                switch (reader["FK_StatusId"].ToString())
                                {
                                    case "1":
                                        monthInfo.In_Arbeit += double.Parse(reader["SumBrutto"].ToString()!);
                                        break;
                                    case "2":
                                        monthInfo.Erledigt += double.Parse(reader["SumBrutto"].ToString()!); break;
                                    case "3":
                                        monthInfo.Offen += double.Parse(reader["SumBrutto"].ToString()!); break;
                                    case "4":
                                        monthInfo.Bezahlt += double.Parse(reader["SumBrutto"].ToString()!); break;
                                }
                            }
                        }
                    }
                    monthsInfo.Add(monthInfo);

                }
                return monthsInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            return null!;
        }

        public ResponseDto OpenPdf(string? po, string? bill, string? billCreatedDate)
        {
            try
            {
                //var path = "C:\\Users\\patri\\Downloads\\Rechnungen aus\\2025-03-14-GL-Invoice-2531.pdf\"";
                var path = _config["PDFFileDirectoryPath"]!.ToString();
                if (po != null)
                {
                    path += "\\Aufträge\\" + po + ".pdf";
                    if (!File.Exists(path))
                        return new ResponseDto { Message = "PO-File not found!", Status = false };
                }
                else if (bill != null && billCreatedDate != null)
                {
                    path += "\\Rechnungen aus\\" + billCreatedDate.Split('T')[0] + $"-GL-Invoice-{bill}.pdf";
                    if (!File.Exists(path))
                        return new ResponseDto { Message = "Rechungs-File not found!", Status = false };
                }
                else
                {
                    return new ResponseDto { Message = "Kein Rechnungsdatum gefunden!", Status = false };
                }
                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true,
                });
                Console.WriteLine("Opened PDF successfully!");
                return new ResponseDto { Message = "Opened PDF successfully!", Status = true };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ResponseDto { Message = ex.Message, Status = false };
            }
        }

        internal List<OrderDto> GetOrdersOfPage(int pageNr)
        {
            var list = new List<OrderDto>();
            try
            {

                MySqlCommand ordersCommand = new MySqlCommand(
                    "SELECT o.*, c.Name AS CustomerName, s.Name AS StatusName " +
                    "FROM `Orders` o " +
                    "JOIN Customers c ON c.CustomerId = o.FK_CustomerId " +
                    "JOIN Status s ON s.StatusId = o.FK_StatusId " +
                    "Order By o.InputDate DESC " +
                    "LIMIT 50 OFFSET @pageCalc",
                    dbConnection);
                ordersCommand.Parameters.AddWithValue("@pageCalc", pageNr * 50);
                using (var reader = ordersCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new OrderDto
                        {
                            Id = int.Parse(reader["OrderId"].ToString()!),
                            InputDate = DateTime.Parse(reader["InputDate"].ToString()!),
                            DeliveryDate = reader["DeliveryDate"]?.ToString()?.Length > 0 ? DateTime.Parse(reader["DeliveryDate"]?.ToString()!) : null!,
                            PaymentDate = reader["PaymentDate"]?.ToString()?.Length > 0 ? DateTime.Parse(reader["PaymentDate"]?.ToString()!) : null!,
                            DocumentNr = int.Parse(reader["DocumentNr"].ToString()!),
                            Brutto = double.Parse(reader["Brutto"].ToString()!),
                            Netto = double.Parse(reader["Netto"].ToString()!),
                            Tax = double.Parse(reader["Tax"].ToString()!),
                            CustomerName = reader["CustomerName"].ToString()!,
                            Bill = int.Parse(reader["Bill"].ToString()!),
                            PO = reader["PO"].ToString()!,
                            Status = reader["StatusName"].ToString()!,
                            UID = reader["UID"].ToString()!,
                            BillCreatedDate = reader["BillCreatedDate"]?.ToString()?.Length > 0 ? DateTime.Parse(reader["BillCreatedDate"]?.ToString()!) : null!
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            return list;
        }

        internal List<OrderDto> GetOrdersFiltered(string filterTxt)
        {
            var orders = GetAllOrders();
            if(int.TryParse(filterTxt, out int numberFilter))
            {
                var filteredOrders = orders.Where(x => x.DocumentNr == numberFilter || x.Bill == numberFilter).ToList();
                if (filteredOrders.Count != 0)
                    return filteredOrders;
            }
                return orders.Where(x => x.PO.Contains(filterTxt) || x.CustomerName.Contains(filterTxt) || (x.UID?.Contains(filterTxt) ?? false) || x.InputDate.ToString().Contains(filterTxt) || (x.DeliveryDate?.ToString().Contains(filterTxt) ?? false) || (x.DeliveryDate?.ToString().Contains(filterTxt) ?? false) || (x.BillCreatedDate?.ToString().Contains(filterTxt) ?? false) || (x.PaymentDate?.ToString().Contains(filterTxt) ?? false)).ToList();
        }
    }
}
