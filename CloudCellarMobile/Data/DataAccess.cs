using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudCellarMobile.Models;
using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace CloudCellarMobile.Data
{
    public static class DataAccess
    {
        public static SqlConnectionStringBuilder builder { get; set; }
        static DataAccess()
        {
            builder = new SqlConnectionStringBuilder()
            {
                DataSource = "s00163774.database.windows.net",
                UserID = "azureuser",
                Password = "S00163774atusligo",
                InitialCatalog = "stocktakeDatabase",
                TrustServerCertificate = true
            };
        }

        //~~~~ Products ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public static ObservableCollection<Product> GetProducts()
        {
            ObservableCollection<Product> AllProducts = new ObservableCollection<Product>();

            Thread executeSQL = new Thread(() =>
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"SELECT Id, Brand, Name, Size, Category, SubCategory, Barcode, FullWeight, EmptyWeight
                                                FROM Products";

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product tempProduct = new Product()
                                {
                                    Id = reader.GetInt32(0),
                                    Brand = reader.GetString(1),
                                    Name = reader.GetString(2),
                                    Size = reader.GetInt32(3),
                                    Category = reader.GetString(4),
                                    SubCategory = reader.GetString(5),
                                    Barcode = reader.GetString(6),
                                    FullWeight = reader.GetInt32(7),
                                    EmptyWeight = reader.GetInt32(8)
                                };

                                AllProducts.Add(tempProduct);
                            }
                        }
                    }

                    connection.Close();
                    connection.Dispose();
                }
            });
            executeSQL.Start();
            executeSQL.Join();

            return AllProducts;
        }

        public static Product GetProductById(int id)
        {
            Product tempProduct = new Product();

            Thread executeSQL = new Thread(() =>
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"SELECT Id, Brand, Name, Size, Category, SubCategory, Barcode, FullWeight, EmptyWeight
                                                FROM Products
                                                WHERE Id = @Id";

                        command.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tempProduct = new Product()
                                {
                                    Id = reader.GetInt32(0),
                                    Brand = reader.GetString(1),
                                    Name = reader.GetString(2),
                                    Size = reader.GetInt32(3),
                                    Category = reader.GetString(4),
                                    SubCategory = reader.GetString(5),
                                    Barcode = reader.GetString(6),
                                    FullWeight = reader.GetInt32(7),
                                    EmptyWeight = reader.GetInt32(8)
                                };
                            }
                        }
                    }

                    connection.Close();
                    connection.Dispose();
                }
            });
            executeSQL.Start();
            executeSQL.Join();

            return tempProduct;
        }

        public static int InsertProduct(Product product)
        {
            int newId = 0;

            Thread executeSQL = new Thread(() =>
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"INSERT INTO Products(Brand, Name, Size, Category, SubCategory, Barcode, FullWeight, EmptyWeight) 
                                                OUTPUT INSERTED.Id
                                                VALUES(@Brand, @Name, @Size, @Category, @SubCategory, @Barcode, @FullWeight, @EmptyWeight)";

                        command.Parameters.AddWithValue("@Brand", product.Brand);
                        command.Parameters.AddWithValue("@Name", product.Name);
                        command.Parameters.AddWithValue("@Size", product.Size);
                        command.Parameters.AddWithValue("@Category", product.Category);
                        command.Parameters.AddWithValue("@SubCategory", product.SubCategory);
                        command.Parameters.AddWithValue("@Barcode", product.Barcode);
                        command.Parameters.AddWithValue("@FullWeight", product.FullWeight);
                        command.Parameters.AddWithValue("@EmptyWeight", product.EmptyWeight);

                        newId = (int)command.ExecuteScalar();
                    }

                    connection.Close();
                    connection.Dispose();
                }
            });
            executeSQL.Start();
            executeSQL.Join();

            return newId;
        }

        public static int UpdateProduct(Product product)
        {
            int rowsUpdated = 0;

            Thread executeSQL = new Thread(() =>
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"UPDATE Products
                                                SET Brand = $Brand AND Name = $Name
                                                AND Size = $Size AND Category = $Category
                                                AND SubCategory = $SubCategory AND Barcode = $Barcode
                                                AND FullWeight = $FullWeight AND EmptyWeight = $EmptyWeight
                                                WHERE Id = $Id";
                        command.Parameters.AddWithValue("Id", product.Id);
                        command.Parameters.AddWithValue("Brand", product.Brand);
                        command.Parameters.AddWithValue("Name", product.Name);
                        command.Parameters.AddWithValue("Size", product.Size);
                        command.Parameters.AddWithValue("Category", product.Category);
                        command.Parameters.AddWithValue("SubCategory", product.SubCategory);
                        command.Parameters.AddWithValue("Barcode", product.Barcode);
                        command.Parameters.AddWithValue("FullWeight", product.FullWeight);
                        command.Parameters.AddWithValue("EmptyWeight", product.EmptyWeight);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            rowsUpdated = command.ExecuteNonQuery();
                        }
                    }

                    connection.Close();
                    connection.Dispose();
                }
            });
            executeSQL.Start();
            executeSQL.Join();

            return rowsUpdated;
        }

        public static int DeleteProduct(int id)
        {
            int rowsUpdated = 0;

            Thread executeSQL = new Thread(() =>
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"DELETE FROM Products
                                                WHERE Id = @Id";
                        command.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            rowsUpdated = command.ExecuteNonQuery();
                        }
                    }

                    connection.Close();
                    connection.Dispose();
                }
            });
            executeSQL.Start();
            executeSQL.Join();

            return rowsUpdated;
        }

        //~~~~ Stocks ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public static ObservableCollection<Stock> GetStocks(int outletId, int auditId)
        {
            ObservableCollection<Stock> AllStocks = new ObservableCollection<Stock>();

            Thread executeSQL = new Thread(() =>
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"SELECT Id, ProductId, OutletId, AuditId, AreaId, LocationId, Measure, Weight, CalculatedMeasure, Time
                                                FROM Stocks
                                                WHERE OutletId = @OutletId AND AuditId = @AuditId";

                        command.Parameters.AddWithValue("@OutletId", outletId);
                        command.Parameters.AddWithValue("@AuditId", auditId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Stock tempStock = new Stock()
                                {
                                    Id = reader.GetInt32(0),
                                    ProductId = reader.GetInt32(1),
                                    OutletId = reader.GetInt32(2),
                                    AuditId = reader.GetInt32(3),
                                    AreaId = reader.GetInt32(4),
                                    LocationId = reader.GetInt32(5),
                                    Measure = reader.GetDouble(6),
                                    Weight = reader.GetInt32(7),
                                    CalculatedMeasure = reader.GetDouble(8),
                                    Time = reader.GetDateTime(9),
                                };

                                AllStocks.Add(tempStock);
                            }
                        }
                    }

                    connection.Close();
                    connection.Dispose();
                }
            });
            executeSQL.Start();
            executeSQL.Join();

            return AllStocks;
        }

        public static int InsertStock(Stock stock)
        {
            int newId = 0;

            Thread executeSQL = new Thread(() =>
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"INSERT INTO Stocks(ProductId, OutletId, AuditId, AreaId, LocationId, Measure, Weight, CalculatedMeasure, Time) 
                                                OUTPUT INSERTED.Id
                                                VALUES(@ProductId, @OutletId, @AuditId, @AreaId, @LocationId, @Measure, @Weight, @CalculatedMeasure, @Time)";

                        command.Parameters.AddWithValue("@ProductId", stock.ProductId);
                        command.Parameters.AddWithValue("@OutletId", stock.OutletId);
                        command.Parameters.AddWithValue("@AuditId", stock.AuditId);
                        command.Parameters.AddWithValue("@AreaId", stock.AreaId);
                        command.Parameters.AddWithValue("@LocationId", stock.LocationId);
                        command.Parameters.AddWithValue("@Measure", stock.Measure);
                        command.Parameters.AddWithValue("@Weight", stock.Weight);
                        command.Parameters.AddWithValue("@CalculatedMeasure", stock.CalculatedMeasure);
                        command.Parameters.AddWithValue("@Time", stock.Time);


                        newId = (int)command.ExecuteScalar();
                    }

                    connection.Close();
                    connection.Dispose();
                }
            });
            executeSQL.Start();
            executeSQL.Join();

            return newId;
        }

        //~~~~ Outlets ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public static ObservableCollection<Outlet> GetOutlets()
        {
            ObservableCollection<Outlet> AllOutlets = new ObservableCollection<Outlet>();

            Thread executeSQL = new Thread(() =>
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"SELECT Id, Name, ContactEmail
                                                FROM Outlets";

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Outlet tempOutlet = new Outlet()
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    ContactEmail = reader.GetString(2)
                                };

                                AllOutlets.Add(tempOutlet);
                            }
                        }
                    }

                    connection.Close();
                    connection.Dispose();
                }
            });
            executeSQL.Start();
            executeSQL.Join();

            return AllOutlets;
        }

        public static int InsertOutlet(Outlet outlet)
        {
            int newId = 0;

            Thread executeSQL = new Thread(() =>
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"INSERT INTO Outlets(Name, ContactEmail) 
                                                OUTPUT INSERTED.Id
                                                VALUES(@Name, @ContactEmail)";

                        command.Parameters.AddWithValue("@Name", outlet.Name);
                        command.Parameters.AddWithValue("@ContactEmail", outlet.ContactEmail);


                        newId = (int)command.ExecuteScalar();
                    }

                    connection.Close();
                    connection.Dispose();
                }
            });
            executeSQL.Start();
            executeSQL.Join();

            return newId;

        }

        //~~~~ Audits ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public static ObservableCollection<Audit> GetAudits(int id)
        {
            ObservableCollection<Audit> AllAudits = new ObservableCollection<Audit>();

            Thread executeSQL = new Thread(() =>
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"SELECT Id, OutletId, AuditNo, StartDate, EndDate
                                                FROM Audits
                                                WHERE OutletId = @OutletId";

                        command.Parameters.AddWithValue("@OutletId", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Audit tempAudit = new Audit()
                                {
                                    Id = reader.GetInt32(0),
                                    OutletId = reader.GetInt32(1),
                                    AuditNo = reader.GetInt32(2),
                                    StartDate = reader.GetDateTime(3),
                                    EndDate = reader.GetDateTime(4)
                                };

                                AllAudits.Add(tempAudit);
                            }
                        }
                    }

                    connection.Close();
                    connection.Dispose();
                }
            });
            executeSQL.Start();
            executeSQL.Join();

            return AllAudits;
        }

        public static int InsertAudit(Audit audit)
        {
            int newId = 0;

            Thread executeSQL = new Thread(() =>
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"INSERT INTO Audits(OutletId, AuditNo, StartDate, EndDate) 
                                                OUTPUT INSERTED.Id
                                                VALUES(@OutletId, @AuditNo, @StartDate, @EndDate)";

                        command.Parameters.AddWithValue("@OutletId", audit.OutletId);
                        command.Parameters.AddWithValue("@AuditNo", audit.AuditNo);
                        command.Parameters.AddWithValue("@StartDate", audit.StartDate);
                        command.Parameters.AddWithValue("@EndDate", audit.EndDate);


                        newId = (int)command.ExecuteScalar();
                    }

                    connection.Close();
                    connection.Dispose();
                }
            });
            executeSQL.Start();
            executeSQL.Join();

            return newId;

        }

        //~~~~ Areas ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public static ObservableCollection<Area> GetAreas(int id)
        {
            ObservableCollection<Area> AllAreas = new ObservableCollection<Area>();

            Thread executeSQL = new Thread(() =>
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"SELECT Id, OutletId, Name
                                                FROM Areas
                                                WHERE OutletId = @OutletId";

                        command.Parameters.AddWithValue("@OutletId", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Area tempArea = new Area()
                                {
                                    Id = reader.GetInt32(0),
                                    OutletId = reader.GetInt32(1),
                                    Name = reader.GetString(2)
                                };

                                AllAreas.Add(tempArea);
                            }
                        }
                    }

                    connection.Close();
                    connection.Dispose();
                }
            });
            executeSQL.Start();
            executeSQL.Join();

            return AllAreas;
        }

        public static int InsertArea(Area Area)
        {
            int newId = 0;

            Thread executeSQL = new Thread(() =>
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"INSERT INTO Areas(OutletId, Name) 
                                                OUTPUT INSERTED.Id
                                                VALUES(@OutletId, @Name)";

                        command.Parameters.AddWithValue("@OutletId", Area.OutletId);
                        command.Parameters.AddWithValue("@Name", Area.Name);


                        newId = (int)command.ExecuteScalar();
                    }

                    connection.Close();
                    connection.Dispose();
                }
            });
            executeSQL.Start();
            executeSQL.Join();

            return newId;

        }

        //~~~~ Locations ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public static ObservableCollection<Models.Location> GetLocations(int id)
        {
            ObservableCollection<Models.Location> AllLocations = new ObservableCollection<Models.Location>();

            Thread executeSQL = new Thread(() =>
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"SELECT Id, AreaId, Name
                                                FROM Locations
                                                WHERE AreaId = @AreaId";

                        command.Parameters.AddWithValue("@AreaId", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.Location tempLocation = new Models.Location()
                                {
                                    Id = reader.GetInt32(0),
                                    AreaId = reader.GetInt32(1),
                                    Name = reader.GetString(2)
                                };

                                AllLocations.Add(tempLocation);
                            }
                        }
                    }

                    connection.Close();
                    connection.Dispose();
                }
            });
            executeSQL.Start();
            executeSQL.Join();

            return AllLocations;
        }

        public static int InsertLocation(Models.Location Location)
        {
            int newId = 0;

            Thread executeSQL = new Thread(() =>
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"INSERT INTO Locations(AreaId, Name) 
                                                OUTPUT INSERTED.Id
                                                VALUES(@AreaId, @Name)";

                        command.Parameters.AddWithValue("@AreaId", Location.AreaId);
                        command.Parameters.AddWithValue("@Name", Location.Name);


                        newId = (int)command.ExecuteScalar();
                    }

                    connection.Close();
                    connection.Dispose();
                }
            });
            executeSQL.Start();
            executeSQL.Join();

            return newId;

        }
    }
}
