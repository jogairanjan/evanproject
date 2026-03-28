using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using vestshed.Models;
using System.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;

namespace vestshed.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Add DbSet properties for your entities here
        // Example:
        // public DbSet<YourEntity> YourEntities { get; set; }

        public async Task<string> InsertProviderOnboardingTempAsync(ProviderOnboardingTempRequest request)
        {
            // Get connection string from the existing connection (just the string, no query execution)
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;

            // Create a completely NEW independent connection to avoid any EF Core tracking/interference
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_InsertProviderOnboardingTemp", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters (no Id parameter since it's identity)
                command.Parameters.Add(new SqlParameter("@CurrentStep", SqlDbType.Int) { Value = request.CurrentStep });
                command.Parameters.Add(new SqlParameter("@ProgressPercentage", SqlDbType.Int) { Value = request.ProgressPercentage });
                command.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar, 50) { Value = (object?)request.Status ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@AccountType", SqlDbType.VarChar, 50) { Value = (object?)request.AccountType ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@BusinessName", SqlDbType.VarChar, 200) { Value = (object?)request.BusinessName ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@OwnerName", SqlDbType.VarChar, 200) { Value = (object?)request.OwnerName ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 150) { Value = (object?)request.Email ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@PhoneNumber", SqlDbType.VarChar, 20) { Value = (object?)request.PhoneNumber ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@PasswordHash", SqlDbType.VarChar, 500) { Value = (object?)request.PasswordHash ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@IsGoogleSignUp", SqlDbType.Bit) { Value = request.IsGoogleSignUp });
                command.Parameters.Add(new SqlParameter("@GoogleId", SqlDbType.VarChar, 200) { Value = (object?)request.GoogleId ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@SelectedServices", SqlDbType.VarChar) { Value = (object?)request.SelectedServices ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@BaseFeeService", SqlDbType.VarChar, 100) { Value = (object?)request.BaseFeeService ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@BaseFeeAmount", SqlDbType.Decimal)
                {
                    Value = (object?)request.BaseFeeAmount ?? DBNull.Value,
                    Precision = 18,
                    Scale = 2
                });
                command.Parameters.Add(new SqlParameter("@TeamSize", SqlDbType.Int) { Value = (object?)request.TeamSize ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@TeamSizeMin", SqlDbType.Int) { Value = (object?)request.TeamSizeMin ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@TeamSizeMax", SqlDbType.Int) { Value = (object?)request.TeamSizeMax ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@NumberOfLocations", SqlDbType.Int) { Value = (object?)request.NumberOfLocations ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@IsMobileService", SqlDbType.Bit) { Value = request.IsMobileService });
                command.Parameters.Add(new SqlParameter("@TierName", SqlDbType.VarChar, 100) { Value = (object?)request.TierName ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@BaseSurchargePerLocation", SqlDbType.Decimal)
                {
                    Value = (object?)request.BaseSurchargePerLocation ?? DBNull.Value,
                    Precision = 18,
                    Scale = 2
                });
                command.Parameters.Add(new SqlParameter("@LocationDiscountPercentage", SqlDbType.Decimal)
                {
                    Value = (object?)request.LocationDiscountPercentage ?? DBNull.Value,
                    Precision = 18,
                    Scale = 2
                });
                command.Parameters.Add(new SqlParameter("@AdminBundleSize", SqlDbType.Int) { Value = (object?)request.AdminBundleSize ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@AdminBundlePrice", SqlDbType.Decimal)
                {
                    Value = (object?)request.AdminBundlePrice ?? DBNull.Value,
                    Precision = 18,
                    Scale = 2
                });
                command.Parameters.Add(new SqlParameter("@AllowProvidersManageSchedule", SqlDbType.Bit) { Value = request.AllowProvidersManageSchedule });
                command.Parameters.Add(new SqlParameter("@LocationSurchargeTotal", SqlDbType.Decimal)
                {
                    Value = (object?)request.LocationSurchargeTotal ?? DBNull.Value,
                    Precision = 18,
                    Scale = 2
                });
                command.Parameters.Add(new SqlParameter("@MultiLocationDiscount", SqlDbType.Decimal)
                {
                    Value = (object?)request.MultiLocationDiscount ?? DBNull.Value,
                    Precision = 18,
                    Scale = 2
                });
                command.Parameters.Add(new SqlParameter("@TotalMonthly", SqlDbType.Decimal)
                {
                    Value = (object?)request.TotalMonthly ?? DBNull.Value,
                    Precision = 18,
                    Scale = 2
                });
                command.Parameters.Add(new SqlParameter("@PricingFormula", SqlDbType.VarChar, 500) { Value = (object?)request.PricingFormula ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@PaymentProvider", SqlDbType.VarChar, 100) { Value = (object?)request.PaymentProvider ?? DBNull.Value });

                using var reader = await command.ExecuteReaderAsync();

                string providerId = string.Empty;

                if (await reader.ReadAsync())
                {
                    if (reader.HasRows && reader.FieldCount > 0)
                    {
                        var providerIdIndex = reader.GetOrdinal("ProviderId");
                        providerId = reader.IsDBNull(providerIdIndex) ? string.Empty : reader.GetString(providerIdIndex);
                    }
                }

                if (string.IsNullOrEmpty(providerId))
                {
                    throw new InvalidOperationException("Stored procedure executed but no ProviderId was returned. The insertion may have failed.");
                }

                return providerId;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
        }

        public async Task<string> MoveProviderOnboardingAsync(MoveProviderOnboardingRequest request)
        {
            // Get connection string from the existing connection (just the string, no query execution)
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            
            // Create a completely NEW independent connection to avoid any EF Core tracking/interference
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("usp_MoveProviderOnboarding", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Required parameter
                command.Parameters.Add(new SqlParameter("@TempProviderId", SqlDbType.VarChar, 50) { Value = request.TempProviderId ?? (object)DBNull.Value });

                // Optional parameters - only add if they have values
                AddOptionalParameter(command, "@PaymentCustomerId", SqlDbType.NVarChar, request.PaymentCustomerId, 100);
                AddOptionalParameter(command, "@PaymentMethodId", SqlDbType.NVarChar, request.PaymentMethodId, 100);
                AddOptionalParameter(command, "@CardLast4", SqlDbType.NVarChar, request.CardLast4, 4);
                AddOptionalParameter(command, "@CardBrand", SqlDbType.NVarChar, request.CardBrand, 20);
                AddOptionalParameter(command, "@CardExpiry", SqlDbType.NVarChar, request.CardExpiry, 10);
                AddOptionalParameter(command, "@BillingZip", SqlDbType.NVarChar, request.BillingZip, 20);
                AddOptionalParameter(command, "@PaymentStatus", SqlDbType.NVarChar, request.PaymentStatus, 50);
                AddOptionalParameter(command, "@TermsAccepted", SqlDbType.Bit, request.TermsAccepted);
                AddOptionalParameter(command, "@TermsAcceptedDate", SqlDbType.DateTimeOffset, request.TermsAcceptedDate);
                AddOptionalParameter(command, "@TermsVersion", SqlDbType.NVarChar, request.TermsVersion, 20);
                AddOptionalParameter(command, "@TermsIPAddress", SqlDbType.NVarChar, request.TermsIPAddress, 50);
                AddOptionalParameter(command, "@Documents", SqlDbType.NVarChar, request.Documents, -1);
                AddOptionalParameter(command, "@IdLicenseStatus", SqlDbType.NVarChar, request.IdLicenseStatus, 50);
                AddOptionalParameter(command, "@IdLicenseUrl", SqlDbType.NVarChar, request.IdLicenseUrl, 1000);
                AddOptionalParameter(command, "@BusinessLicenseStatus", SqlDbType.NVarChar, request.BusinessLicenseStatus, 50);
                AddOptionalParameter(command, "@BusinessLicenseUrl", SqlDbType.NVarChar, request.BusinessLicenseUrl, 1000);
                AddOptionalParameter(command, "@InsuranceCertStatus", SqlDbType.NVarChar, request.InsuranceCertStatus, 50);
                AddOptionalParameter(command, "@InsuranceCertUrl", SqlDbType.NVarChar, request.InsuranceCertUrl, 1000);
                AddOptionalParameter(command, "@BackgroundCheckStatus", SqlDbType.NVarChar, request.BackgroundCheckStatus, 50);
                AddOptionalParameter(command, "@CheckrCandidateId", SqlDbType.NVarChar, request.CheckrCandidateId, 100);
                AddOptionalParameter(command, "@CheckrReportId", SqlDbType.NVarChar, request.CheckrReportId, 100);
                AddOptionalParameter(command, "@IdentityVerified", SqlDbType.Bit, request.IdentityVerified);
                AddOptionalParameter(command, "@CriminalCheckPassed", SqlDbType.Bit, request.CriminalCheckPassed);
                AddOptionalParameter(command, "@SexOffenderCheckPassed", SqlDbType.Bit, request.SexOffenderCheckPassed);
                AddOptionalParameter(command, "@ReferenceCheckPassed", SqlDbType.Bit, request.ReferenceCheckPassed);
                AddOptionalParameter(command, "@BackgroundCheckDate", SqlDbType.DateTimeOffset, request.BackgroundCheckDate);
                AddOptionalParameter(command, "@ServiceProviderName", SqlDbType.NVarChar, request.ServiceProviderName, 200);
                AddOptionalParameter(command, "@BusinessLogo", SqlDbType.NVarChar, request.BusinessLogo, 1000);
                AddOptionalParameter(command, "@SmallDescription", SqlDbType.NVarChar, request.SmallDescription, 150);
                AddOptionalParameter(command, "@Specialties", SqlDbType.NVarChar, request.Specialties, -1);
                AddOptionalParameter(command, "@HoursOfOperation", SqlDbType.NVarChar, request.HoursOfOperation, 200);
                AddOptionalParameter(command, "@HasInsuranceCoverage", SqlDbType.Bit, request.HasInsuranceCoverage);
                AddOptionalParameter(command, "@EnableGeolocationMap", SqlDbType.Bit, request.EnableGeolocationMap);
                AddOptionalParameter(command, "@Latitude", SqlDbType.Decimal, request.Latitude, null, 10, 8);
                AddOptionalParameter(command, "@Longitude", SqlDbType.Decimal, request.Longitude, null, 11, 8);
                AddOptionalParameter(command, "@Address", SqlDbType.NVarChar, request.Address, 500);
                AddOptionalParameter(command, "@City", SqlDbType.NVarChar, request.City, 100);
                AddOptionalParameter(command, "@State", SqlDbType.NVarChar, request.State, 100);
                AddOptionalParameter(command, "@ZipCode", SqlDbType.NVarChar, request.ZipCode, 20);
                AddOptionalParameter(command, "@ProfileCompleteness", SqlDbType.Int, request.ProfileCompleteness);
                AddOptionalParameter(command, "@AvailableDays", SqlDbType.NVarChar, request.AvailableDays, -1);
                AddOptionalParameter(command, "@ScheduleStartTime", SqlDbType.Time, request.ScheduleStartTime);
                AddOptionalParameter(command, "@ScheduleEndTime", SqlDbType.Time, request.ScheduleEndTime);
                AddOptionalParameter(command, "@SlotDurationMinutes", SqlDbType.Int, request.SlotDurationMinutes);
                AddOptionalParameter(command, "@SlotCapacity", SqlDbType.Int, request.SlotCapacity);
                AddOptionalParameter(command, "@EnableWaitlist", SqlDbType.Bit, request.EnableWaitlist);
                AddOptionalParameter(command, "@EnableGPSTagging", SqlDbType.Bit, request.EnableGPSTagging);
                AddOptionalParameter(command, "@AutoConfirmBookings", SqlDbType.Bit, request.AutoConfirmBookings);
                AddOptionalParameter(command, "@RequireDeposit", SqlDbType.Bit, request.RequireDeposit);
                AddOptionalParameter(command, "@DepositPercentage", SqlDbType.Decimal, request.DepositPercentage, null, 5, 2);
                AddOptionalParameter(command, "@CancellationPolicyHours", SqlDbType.Int, request.CancellationPolicyHours);
                AddOptionalParameter(command, "@CancellationFeePercentage", SqlDbType.Decimal, request.CancellationFeePercentage, null, 5, 2);
                AddOptionalParameter(command, "@SendReminderEmails", SqlDbType.Bit, request.SendReminderEmails);
                AddOptionalParameter(command, "@ReminderHoursBefore", SqlDbType.Int, request.ReminderHoursBefore);
                AddOptionalParameter(command, "@AllowRescheduling", SqlDbType.Bit, request.AllowRescheduling);
                AddOptionalParameter(command, "@RescheduleHoursBefore", SqlDbType.Int, request.RescheduleHoursBefore);
                AddOptionalParameter(command, "@RequireNewClientApproval", SqlDbType.Bit, request.RequireNewClientApproval);
                AddOptionalParameter(command, "@IsReviewed", SqlDbType.Bit, request.IsReviewed);
                AddOptionalParameter(command, "@ReviewedDate", SqlDbType.DateTimeOffset, request.ReviewedDate);
                AddOptionalParameter(command, "@IsApproved", SqlDbType.Bit, request.IsApproved);
                AddOptionalParameter(command, "@ApprovedDate", SqlDbType.DateTimeOffset, request.ApprovedDate);
                AddOptionalParameter(command, "@ApprovedBy", SqlDbType.Int, request.ApprovedBy);
                AddOptionalParameter(command, "@RejectionReason", SqlDbType.NVarChar, request.RejectionReason, 500);
                AddOptionalParameter(command, "@IsLive", SqlDbType.Bit, request.IsLive);
                AddOptionalParameter(command, "@GoLiveDate", SqlDbType.DateTimeOffset, request.GoLiveDate);
                AddOptionalParameter(command, "@WelcomeEmailSent", SqlDbType.Bit, request.WelcomeEmailSent);
                AddOptionalParameter(command, "@UserId", SqlDbType.Int, request.UserId);
                AddOptionalParameter(command, "@StartedDate", SqlDbType.DateTimeOffset, request.StartedDate);
                AddOptionalParameter(command, "@LastActivityDate", SqlDbType.DateTimeOffset, request.LastActivityDate);
                AddOptionalParameter(command, "@CompletedDate", SqlDbType.DateTimeOffset, request.CompletedDate);
                AddOptionalParameter(command, "@CreatedDate", SqlDbType.DateTimeOffset, request.CreatedDate);
                AddOptionalParameter(command, "@ModifiedDate", SqlDbType.DateTimeOffset, request.ModifiedDate);

                // Execute the stored procedure and read the result
                using var reader = await command.ExecuteReaderAsync();
                
                string newProviderId = string.Empty;
                
                if (await reader.ReadAsync())
                {
                    if (reader.HasRows && reader.FieldCount > 0)
                    {
                        var providerIdIndex = reader.GetOrdinal("NewProviderId");
                        newProviderId = reader.IsDBNull(providerIdIndex) ? string.Empty : reader.GetString(providerIdIndex);
                    }
                }

                if (string.IsNullOrEmpty(newProviderId))
                {
                    throw new InvalidOperationException("Stored procedure executed but no NewProviderId was returned. The move operation may have failed.");
                }

                return newProviderId;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        // Helper method to add optional parameters
        private void AddOptionalParameter(SqlCommand command, string parameterName, SqlDbType dbType, object? value, int? size = null, byte? precision = null, byte? scale = null)
        {
            var parameter = new SqlParameter(parameterName, dbType);
            
            if (size.HasValue && size.Value > 0)
            {
                parameter.Size = size.Value;
            }
            
            if (precision.HasValue && scale.HasValue)
            {
                parameter.Precision = precision.Value;
                parameter.Scale = scale.Value;
            }
            
            parameter.Value = value ?? (object)DBNull.Value;
            command.Parameters.Add(parameter);
        }

        public async Task<object> EmployeesCRUDAsync(string action, EmployeeRequest request)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            
            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_Employees_CRUD", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Action parameter (required)
                command.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 50) { Value = action });

                // Id parameter (for UPDATE, DELETE, GETBYID)
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = request.Id ?? (object)DBNull.Value });

                // Add all other parameters
                AddOptionalParameter(command, "@EmployeeCode", SqlDbType.NVarChar, request.EmployeeCode, 20);
                
                // Handle Serviceproviderid - convert object to string
                string? serviceProviderIdString = null;
                if (request.Serviceproviderid != null)
                {
                    serviceProviderIdString = request.Serviceproviderid.ToString();
                }
                AddOptionalParameter(command, "@Serviceproviderid", SqlDbType.NVarChar, serviceProviderIdString, 20);
                AddOptionalParameter(command, "@FirstName", SqlDbType.NVarChar, request.FirstName, 100);
                AddOptionalParameter(command, "@LastName", SqlDbType.NVarChar, request.LastName, 100);
                AddOptionalParameter(command, "@FullName", SqlDbType.NVarChar, request.FullName, 200);
                AddOptionalParameter(command, "@Email", SqlDbType.NVarChar, request.Email, 256);
                AddOptionalParameter(command, "@PhoneNumber", SqlDbType.NVarChar, request.PhoneNumber, 20);
                AddOptionalParameter(command, "@AlternatePhone", SqlDbType.NVarChar, request.AlternatePhone, 20);
                AddOptionalParameter(command, "@ProfileImage", SqlDbType.NVarChar, request.ProfileImage, 1000);
                AddOptionalParameter(command, "@DateOfBirth", SqlDbType.Date, request.DateOfBirth);
                AddOptionalParameter(command, "@Gender", SqlDbType.NVarChar, request.Gender, 20);
                AddOptionalParameter(command, "@Address", SqlDbType.NVarChar, request.Address, 500);
                AddOptionalParameter(command, "@City", SqlDbType.NVarChar, request.City, 100);
                AddOptionalParameter(command, "@State", SqlDbType.NVarChar, request.State, 100);
                AddOptionalParameter(command, "@ZipCode", SqlDbType.NVarChar, request.ZipCode, 20);
                AddOptionalParameter(command, "@Country", SqlDbType.NVarChar, request.Country, 100);
                AddOptionalParameter(command, "@EmployeeType", SqlDbType.NVarChar, request.EmployeeType, 50);
                AddOptionalParameter(command, "@Department", SqlDbType.NVarChar, request.Department, 100);
                AddOptionalParameter(command, "@Position", SqlDbType.NVarChar, request.Position, 100);
                AddOptionalParameter(command, "@JobTitle", SqlDbType.NVarChar, request.JobTitle, 100);
                AddOptionalParameter(command, "@ReportsTo", SqlDbType.Int, request.ReportsTo);
                AddOptionalParameter(command, "@HireDate", SqlDbType.Date, request.HireDate);
                AddOptionalParameter(command, "@StartDate", SqlDbType.Date, request.StartDate);
                AddOptionalParameter(command, "@EndDate", SqlDbType.Date, request.EndDate);
                AddOptionalParameter(command, "@ProbationEndDate", SqlDbType.Date, request.ProbationEndDate);
                AddOptionalParameter(command, "@WorkSchedule", SqlDbType.NVarChar, request.WorkSchedule, -1);
                AddOptionalParameter(command, "@DefaultStartTime", SqlDbType.Time, request.DefaultStartTime);
                AddOptionalParameter(command, "@DefaultEndTime", SqlDbType.Time, request.DefaultEndTime);
                AddOptionalParameter(command, "@WeeklyHours", SqlDbType.Decimal, request.WeeklyHours, null, 5, 2);
                AddOptionalParameter(command, "@IsFlexibleSchedule", SqlDbType.Bit, request.IsFlexibleSchedule);
                AddOptionalParameter(command, "@PayType", SqlDbType.NVarChar, request.PayType, 20);
                AddOptionalParameter(command, "@PayRate", SqlDbType.Decimal, request.PayRate, null, 10, 2);
                AddOptionalParameter(command, "@Currency", SqlDbType.NVarChar, request.Currency, 10);
                AddOptionalParameter(command, "@BankAccountNumber", SqlDbType.NVarChar, request.BankAccountNumber, 50);
                AddOptionalParameter(command, "@BankRoutingNumber", SqlDbType.NVarChar, request.BankRoutingNumber, 50);
                AddOptionalParameter(command, "@Skills", SqlDbType.NVarChar, request.Skills, -1);
                AddOptionalParameter(command, "@Certifications", SqlDbType.NVarChar, request.Certifications, -1);
                AddOptionalParameter(command, "@Specializations", SqlDbType.NVarChar, request.Specializations, -1);
                AddOptionalParameter(command, "@Languages", SqlDbType.NVarChar, request.Languages, 500);
                AddOptionalParameter(command, "@YearsOfExperience", SqlDbType.Int, request.YearsOfExperience);
                AddOptionalParameter(command, "@Bio", SqlDbType.NVarChar, request.Bio, 1000);
                AddOptionalParameter(command, "@AssignedServices", SqlDbType.NVarChar, request.AssignedServices, -1);
                AddOptionalParameter(command, "@AssignedLocations", SqlDbType.NVarChar, request.AssignedLocations, -1);
                AddOptionalParameter(command, "@MaxDailyAppointments", SqlDbType.Int, request.MaxDailyAppointments);
                AddOptionalParameter(command, "@CanAcceptNewClients", SqlDbType.Bit, request.CanAcceptNewClients);
                AddOptionalParameter(command, "@UserId", SqlDbType.Int, request.UserId);
                AddOptionalParameter(command, "@Role", SqlDbType.NVarChar, request.Role, 50);
                AddOptionalParameter(command, "@Permissions", SqlDbType.NVarChar, request.Permissions, -1);
                AddOptionalParameter(command, "@CanManageOwnSchedule", SqlDbType.Bit, request.CanManageOwnSchedule);
                AddOptionalParameter(command, "@CanViewReports", SqlDbType.Bit, request.CanViewReports);
                AddOptionalParameter(command, "@CanManageClients", SqlDbType.Bit, request.CanManageClients);
                AddOptionalParameter(command, "@LastLoginDate", SqlDbType.DateTimeOffset, request.LastLoginDate);
                AddOptionalParameter(command, "@IdDocumentUrl", SqlDbType.NVarChar, request.IdDocumentUrl, 1000);
                AddOptionalParameter(command, "@IdDocumentVerified", SqlDbType.Bit, request.IdDocumentVerified);
                AddOptionalParameter(command, "@BackgroundCheckStatus", SqlDbType.NVarChar, request.BackgroundCheckStatus, 50);
                AddOptionalParameter(command, "@BackgroundCheckDate", SqlDbType.Date, request.BackgroundCheckDate);
                AddOptionalParameter(command, "@EmergencyContactName", SqlDbType.NVarChar, request.EmergencyContactName, 200);
                AddOptionalParameter(command, "@EmergencyContactPhone", SqlDbType.NVarChar, request.EmergencyContactPhone, 20);
                AddOptionalParameter(command, "@EmergencyContactRelation", SqlDbType.NVarChar, request.EmergencyContactRelation, 50);
                AddOptionalParameter(command, "@Status", SqlDbType.NVarChar, request.Status, 50);
                AddOptionalParameter(command, "@StatusReason", SqlDbType.NVarChar, request.StatusReason, 500);
                AddOptionalParameter(command, "@Notes", SqlDbType.NVarChar, request.Notes, -1);
                AddOptionalParameter(command, "@Rating", SqlDbType.Decimal, request.Rating, null, 3, 2);
                AddOptionalParameter(command, "@TotalReviews", SqlDbType.Int, request.TotalReviews);
                AddOptionalParameter(command, "@ProviderId", SqlDbType.Int, request.ProviderId);
                AddOptionalParameter(command, "@CreatedBy", SqlDbType.Int, request.CreatedBy);
                AddOptionalParameter(command, "@ModifiedBy", SqlDbType.Int, request.ModifiedBy);

                // Execute the stored procedure
                using var reader = await command.ExecuteReaderAsync();
                
                // Handle different return types based on action
                if (action == "INSERT")
                {
                    if (await reader.ReadAsync())
                    {
                        var newEmployeeIdIndex = reader.GetOrdinal("NewEmployeeId");
                        var newEmployeeId = reader.IsDBNull(newEmployeeIdIndex) ? (int?)null : reader.GetInt32(newEmployeeIdIndex);
                        return newEmployeeId ?? 0;
                    }
                    return 0;
                }
                else if (action == "UPDATE" || action == "DELETE")
                {
                    if (await reader.ReadAsync())
                    {
                        var messageIndex = reader.GetOrdinal("Message");
                        return reader.IsDBNull(messageIndex) ? string.Empty : reader.GetString(messageIndex);
                    }
                    return action == "UPDATE" ? "Updated Successfully" : "Deleted Successfully";
                }
                else if (action == "GETBYID")
                {
                    if (await reader.ReadAsync())
                    {
                        var employee = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            employee[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        return employee;
                    }
                    return null;
                }
                else if (action == "GETALL" || action == "GETBYSERVICEPROVIDERID")
                {
                    var employees = new List<Dictionary<string, object?>>();
                    while (await reader.ReadAsync())
                    {
                        var employee = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            employee[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        employees.Add(employee);
                    }
                    return employees;
                }

                return null;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        public async Task<object> ServicesCRUDAsync(string action, ServiceRequest request)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            
            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_Services_CRUD", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Action parameter (required)
                command.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 50) { Value = action });

                // Id parameter (for UPDATE, DELETE, GETBYID)
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = request.Id > 0 ? request.Id : (object)DBNull.Value });

                // Add all other parameters
                AddOptionalParameter(command, "@ServiceProviderId", SqlDbType.Int, request.ServiceProviderId);
                AddOptionalParameter(command, "@ServiceName", SqlDbType.NVarChar, request.ServiceName, 200);
                AddOptionalParameter(command, "@Description", SqlDbType.NVarChar, request.Description, -1);
                AddOptionalParameter(command, "@Pricing", SqlDbType.Decimal, request.Pricing, null, 10, 2);
                AddOptionalParameter(command, "@StartTime", SqlDbType.Time, request.StartTime != null && TimeSpan.TryParse(request.StartTime, out var startTs) ? startTs : (object?)null);
                AddOptionalParameter(command, "@EndTime", SqlDbType.Time, request.EndTime != null && TimeSpan.TryParse(request.EndTime, out var endTs) ? endTs : (object?)null);

                // Handle SubServices XML
                if (request.SubServices != null && request.SubServices.Count > 0)
                {
                    var subServicesXml = $"<SubServices>{string.Join("", request.SubServices.Select(s =>
                        $"<SubService><SubServiceName>{System.Net.WebUtility.HtmlEncode(s.SubServiceName)}</SubServiceName>" +
                        $"<Price>{s.Price}</Price>" +
                        $"<Name>{System.Net.WebUtility.HtmlEncode(s.Name)}</Name>" +
                        $"<Offered>{(s.Offered ? 1 : 0)}</Offered></SubService>"))}</SubServices>";
                    command.Parameters.Add(new SqlParameter("@SubServices", SqlDbType.NVarChar, -1) { Value = subServicesXml });
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@SubServices", SqlDbType.NVarChar, -1) { Value = DBNull.Value });
                }

                // Handle Assignments XML
                if (request.Assignments != null && request.Assignments.Count > 0)
                {
                    var assignmentsXml = $"<Assignments>{string.Join("", request.Assignments.Select(a =>
                        $"<Item><EmployeeId>{a.EmployeeId}</EmployeeId>" +
                        $"<LocationId>{a.LocationId}</LocationId></Item>"))}</Assignments>";
                    command.Parameters.Add(new SqlParameter("@Assignments", SqlDbType.NVarChar, -1) { Value = assignmentsXml });
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@Assignments", SqlDbType.NVarChar, -1) { Value = DBNull.Value });
                }

                // Execute the stored procedure
                using var reader = await command.ExecuteReaderAsync();
                
                // Handle different return types based on action
                if (action == "INSERT")
                {
                    if (await reader.ReadAsync())
                    {
                        var serviceIdIndex = reader.GetOrdinal("ServiceId");
                        var serviceId = reader.IsDBNull(serviceIdIndex) ? (int?)null : Convert.ToInt32(reader.GetValue(serviceIdIndex));
                        return serviceId ?? 0;
                    }
                    return 0;
                }
                else if (action == "UPDATE" || action == "DELETE")
                {
                    if (await reader.ReadAsync())
                    {
                        var messageIndex = reader.GetOrdinal("Message");
                        return reader.IsDBNull(messageIndex) ? string.Empty : reader.GetString(messageIndex);
                    }
                    return action == "UPDATE" ? "Updated Successfully" : "Deleted Successfully";
                }
                else if (action == "GETBYID")
                {
                    var result = new Dictionary<string, object?>();
                    
                    // Read Services table
                    if (await reader.ReadAsync())
                    {
                        var service = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            service[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        result["Service"] = service;
                    }
                    
                    // Read SubServices table
                    await reader.NextResultAsync();
                    var subServices = new List<Dictionary<string, object?>>();
                    while (await reader.ReadAsync())
                    {
                        var subService = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            subService[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        subServices.Add(subService);
                    }
                    result["SubServices"] = subServices;
                    
                    // Read ServiceAssignments table
                    await reader.NextResultAsync();
                    var assignments = new List<Dictionary<string, object?>>();
                    while (await reader.ReadAsync())
                    {
                        var assignment = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            assignment[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        assignments.Add(assignment);
                    }
                    result["Assignments"] = assignments;
                    
                    return result;
                }
                else if (action == "GETALL" || action == "GETBYSERVICEPROVIDERID")
                {
                    var services = new List<Dictionary<string, object?>>();
                    while (await reader.ReadAsync())
                    {
                        var service = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            service[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        services.Add(service);
                    }
                    return services;
                }

                return null;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        public async Task<int> InsertPetParentAsync(PetParentRequest request)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            
            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_PetParents_Insert", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Required parameters
                command.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 100) { Value = request.FirstName ?? (object)DBNull.Value });
                command.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 100) { Value = request.LastName ?? (object)DBNull.Value });
                command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 256) { Value = request.Email ?? (object)DBNull.Value });

                // Optional parameters
                AddOptionalParameter(command, "@PhoneNumber", SqlDbType.NVarChar, request.PhoneNumber, 20);
                AddOptionalParameter(command, "@Address", SqlDbType.NVarChar, request.Address, 500);
                AddOptionalParameter(command, "@City", SqlDbType.NVarChar, request.City, 100);
                AddOptionalParameter(command, "@State", SqlDbType.NVarChar, request.State, 100);
                AddOptionalParameter(command, "@Zip", SqlDbType.NVarChar, request.Zip, 20);
                AddOptionalParameter(command, "@PreferredContactMethod", SqlDbType.NVarChar, request.PreferredContactMethod, 20);
                AddOptionalParameter(command, "@ServiceProviderId", SqlDbType.Int, request.ServiceProviderId);
                AddOptionalParameter(command, "@IsActive", SqlDbType.Bit, request.IsActive);
                AddOptionalParameter(command, "@Password", SqlDbType.VarChar, request.Password, 30);
                AddOptionalParameter(command, "@AccountSecurity", SqlDbType.VarChar, request.AccountSecurity, 20);

                // Execute the stored procedure and read the result
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    if (reader.HasRows && reader.FieldCount > 0)
                    {
                        var newPetParentIdIndex = reader.GetOrdinal("NewPetParentId");
                        var newPetParentId = reader.IsDBNull(newPetParentIdIndex) ? 0 : reader.GetInt32(newPetParentIdIndex);
                        return newPetParentId;
                    }
                }

                return 0;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        public async Task<object> GetPetParentsAsync(int? id = null)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            
            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_PetParents_Get", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Id parameter (NULL = Get all, NOT NULL = Get specific record)
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id ?? (object)DBNull.Value });

                // Execute the stored procedure
                using var reader = await command.ExecuteReaderAsync();
                
                if (id.HasValue)
                {
                    // Get single record
                    if (await reader.ReadAsync())
                    {
                        var petParent = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            petParent[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        return petParent;
                    }
                    return null;
                }
                else
                {
                    // Get all records
                    var petParents = new List<Dictionary<string, object?>>();
                    while (await reader.ReadAsync())
                    {
                        var petParent = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            petParent[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        petParents.Add(petParent);
                    }
                    return petParents;
                }
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        public async Task<object> PetsCRUDAsync(string action, PetRequest request)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            
            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_Pets_CRUD", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Action parameter (required)
                command.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 30) { Value = action });

                // Id parameter (for UPDATE, DELETE, GETBYID)
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = request.Id ?? (object)DBNull.Value });

                // Add all other parameters
                AddOptionalParameter(command, "@PetName", SqlDbType.NVarChar, request.PetName, 100);
                AddOptionalParameter(command, "@PetParentId", SqlDbType.Int, request.PetParentId);
                AddOptionalParameter(command, "@Species", SqlDbType.NVarChar, request.Species, 100);
                AddOptionalParameter(command, "@Breed", SqlDbType.NVarChar, request.Breed, 100);
                AddOptionalParameter(command, "@Age", SqlDbType.Int, request.Age);
                AddOptionalParameter(command, "@Sex", SqlDbType.NVarChar, request.Sex, 20);
                AddOptionalParameter(command, "@MicrochipId", SqlDbType.NVarChar, request.MicrochipId, 100);
                AddOptionalParameter(command, "@Allergies", SqlDbType.NVarChar, request.Allergies, -1);
                AddOptionalParameter(command, "@Medications", SqlDbType.NVarChar, request.Medications, -1);
                AddOptionalParameter(command, "@ServiceProviderId", SqlDbType.Int, request.ServiceProviderId);
                AddOptionalParameter(command, "@IsActive", SqlDbType.Bit, request.IsActive);
                AddOptionalParameter(command, "@Spayed", SqlDbType.Bit, request.Spayed);
                AddOptionalParameter(command, "@Microchipped", SqlDbType.Bit, request.Microchipped);

                // Execute the stored procedure
                using var reader = await command.ExecuteReaderAsync();
                
                // Handle different return types based on action
                if (action == "INSERT")
                {
                    if (await reader.ReadAsync())
                    {
                        var newPetIdIndex = reader.GetOrdinal("NewPetId");
                        var newPetId = reader.IsDBNull(newPetIdIndex) ? (int?)null : reader.GetInt32(newPetIdIndex);
                        return newPetId ?? 0;
                    }
                    return 0;
                }
                else if (action == "UPDATE" || action == "DELETE")
                {
                    if (await reader.ReadAsync())
                    {
                        var messageIndex = reader.GetOrdinal("Message");
                        return reader.IsDBNull(messageIndex) ? string.Empty : reader.GetString(messageIndex);
                    }
                    return action == "UPDATE" ? "Updated Successfully" : "Deleted Successfully";
                }
                else if (action == "GETBYID")
                {
                    if (await reader.ReadAsync())
                    {
                        var pet = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            pet[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        return pet;
                    }
                    return null;
                }
                else if (action == "GETBYPETPARENTID")
                {
                    var pets = new List<Dictionary<string, object?>>();
                    while (await reader.ReadAsync())
                    {
                        var pet = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            pet[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        pets.Add(pet);
                    }
                    return pets;
                }
                else if (action == "GETALL")
                {
                    var pets = new List<Dictionary<string, object?>>();
                    while (await reader.ReadAsync())
                    {
                        var pet = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            pet[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        pets.Add(pet);
                    }
                    return pets;
                }

                return null;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        public async Task<object> ServiceOrdersCRUDAsync(string action, ServiceOrderRequest request)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            
            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_ServiceOrders_CRUD", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Action parameter (required)
                command.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 30) { Value = action });

                // Id parameter (for UPDATE, DELETE, GETBYID)
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = request.Id ?? (object)DBNull.Value });

                // Add all other parameters
                AddOptionalParameter(command, "@ServiceProviderId", SqlDbType.Int, request.ServiceProviderId);
                AddOptionalParameter(command, "@PetParentId", SqlDbType.Int, request.PetParentId);
                AddOptionalParameter(command, "@LocationId", SqlDbType.Int, request.LocationId);
                AddOptionalParameter(command, "@Items", SqlDbType.NVarChar, request.Items, -1);
                AddOptionalParameter(command, "@Quantity", SqlDbType.NVarChar, request.Quantity, -1);
                AddOptionalParameter(command, "@Amount", SqlDbType.NVarChar, request.Amount, -1);
                AddOptionalParameter(command, "@CareNotes", SqlDbType.NVarChar, request.CareNotes, -1);
                AddOptionalParameter(command, "@PaymentTerms", SqlDbType.NVarChar, request.PaymentTerms, 500);
                AddOptionalParameter(command, "@DepositPercentage", SqlDbType.Decimal, request.DepositPercentage, null, 5, 2);
                AddOptionalParameter(command, "@AnyTip", SqlDbType.Decimal, request.AnyTip, null, 10, 2);
                AddOptionalParameter(command, "@RequestData", SqlDbType.NVarChar, request.RequestData, -1);
                AddOptionalParameter(command, "@ResponseData", SqlDbType.NVarChar, request.ResponseData, -1);

                // Execute the stored procedure
                using var reader = await command.ExecuteReaderAsync();
                
                // Handle different return types based on action
                if (action == "INSERT")
                {
                    if (await reader.ReadAsync())
                    {
                        var newServiceOrderIdIndex = reader.GetOrdinal("NewServiceOrderId");
                        var newServiceOrderId = reader.IsDBNull(newServiceOrderIdIndex) ? (int?)null : reader.GetInt32(newServiceOrderIdIndex);
                        return newServiceOrderId ?? 0;
                    }
                    return 0;
                }
                else if (action == "UPDATE" || action == "DELETE")
                {
                    if (await reader.ReadAsync())
                    {
                        var messageIndex = reader.GetOrdinal("Message");
                        return reader.IsDBNull(messageIndex) ? string.Empty : reader.GetString(messageIndex);
                    }
                    return action == "UPDATE" ? "Updated Successfully" : "Deleted Successfully";
                }
                else if (action == "GETBYID")
                {
                    if (await reader.ReadAsync())
                    {
                        var serviceOrder = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            serviceOrder[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        return serviceOrder;
                    }
                    return null;
                }
                else if (action == "GETBYPETPARENTID")
                {
                    var serviceOrders = new List<Dictionary<string, object?>>();
                    while (await reader.ReadAsync())
                    {
                        var serviceOrder = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            serviceOrder[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        serviceOrders.Add(serviceOrder);
                    }
                    return serviceOrders;
                }
                else if (action == "GETALL")
                {
                    var serviceOrders = new List<Dictionary<string, object?>>();
                    while (await reader.ReadAsync())
                    {
                        var serviceOrder = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            serviceOrder[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        serviceOrders.Add(serviceOrder);
                    }
                    return serviceOrders;
                }

                return null;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        public async Task<int> ServiceFeedbackInsertAsync(ServiceFeedbackInsertRequest request)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            
            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_ServiceFeedback_Insert", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Required parameters
                command.Parameters.Add(new SqlParameter("@EmployeeId", SqlDbType.Int) { Value = request.EmployeeId });
                command.Parameters.Add(new SqlParameter("@PetParentId", SqlDbType.Int) { Value = request.PetParentId });
                command.Parameters.Add(new SqlParameter("@PetId", SqlDbType.Int) { Value = request.PetId });
                command.Parameters.Add(new SqlParameter("@BookingDate", SqlDbType.Date) { Value = request.BookingDate });
                command.Parameters.Add(new SqlParameter("@BookingTime", SqlDbType.Time) { Value = request.BookingTime });
                command.Parameters.Add(new SqlParameter("@CheckInStatus", SqlDbType.NVarChar, 50) { Value = request.CheckInStatus ?? (object)DBNull.Value });
                command.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 50) { Value = request.Status ?? (object)DBNull.Value });

                // Execute the stored procedure and read the result
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    if (reader.HasRows && reader.FieldCount > 0)
                    {
                        var newIdIndex = reader.GetOrdinal("NewId");
                        var newId = reader.IsDBNull(newIdIndex) ? 0 : reader.GetInt32(newIdIndex);
                        return newId;
                    }
                }

                return 0;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        public async Task<string> ServiceFeedbackUpdateLocationEmployeeAsync(ServiceFeedbackUpdateLocationEmployeeRequest request)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            
            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_ServiceFeedback_UpdateLocationEmployee", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Required parameters
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = request.Id });
                command.Parameters.Add(new SqlParameter("@LocationId", SqlDbType.Int) { Value = request.LocationId });
                command.Parameters.Add(new SqlParameter("@EmployeeId", SqlDbType.Int) { Value = request.EmployeeId });

                // Execute the stored procedure and read the result
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    var messageIndex = reader.GetOrdinal("Message");
                    return reader.IsDBNull(messageIndex) ? string.Empty : reader.GetString(messageIndex);
                }

                return "Location & Employee updated";
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        public async Task<string> ServiceFeedbackUpdateBookingAsync(ServiceFeedbackUpdateBookingRequest request)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            
            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_ServiceFeedback_UpdateBooking", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Required parameters
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = request.Id });
                command.Parameters.Add(new SqlParameter("@BookingDate", SqlDbType.Date) { Value = request.BookingDate });
                command.Parameters.Add(new SqlParameter("@BookingTime", SqlDbType.Time) { Value = request.BookingTime });

                // Execute the stored procedure and read the result
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    var messageIndex = reader.GetOrdinal("Message");
                    return reader.IsDBNull(messageIndex) ? string.Empty : reader.GetString(messageIndex);
                }

                return "Booking date & time updated";
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        public async Task<string> ServiceFeedbackUpdateCheckInAsync(ServiceFeedbackUpdateCheckInRequest request)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            
            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_ServiceFeedback_UpdateCheckIn", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Required parameters
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = request.Id });
                command.Parameters.Add(new SqlParameter("@CheckInDate", SqlDbType.Date) { Value = request.CheckInDate });
                command.Parameters.Add(new SqlParameter("@CheckInTime", SqlDbType.Time) { Value = request.CheckInTime });

                // Execute the stored procedure and read the result
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    var messageIndex = reader.GetOrdinal("Message");
                    return reader.IsDBNull(messageIndex) ? string.Empty : reader.GetString(messageIndex);
                }

                return "Check-in updated";
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        public async Task<string> ServiceFeedbackUpdateCheckOutAsync(ServiceFeedbackUpdateCheckOutRequest request)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            
            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_ServiceFeedback_UpdateCheckOut", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Required parameters
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = request.Id });
                command.Parameters.Add(new SqlParameter("@CheckOutDate", SqlDbType.Date) { Value = request.CheckOutDate });
                command.Parameters.Add(new SqlParameter("@CheckOutTime", SqlDbType.Time) { Value = request.CheckOutTime });

                // Execute the stored procedure and read the result
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    var messageIndex = reader.GetOrdinal("Message");
                    return reader.IsDBNull(messageIndex) ? string.Empty : reader.GetString(messageIndex);
                }

                return "Check-out updated";
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        public async Task<string> ServiceFeedbackUpdateRatingsAsync(ServiceFeedbackUpdateRatingsRequest request)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            
            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_ServiceFeedback_UpdateRatings", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Required parameters
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = request.Id });
                command.Parameters.Add(new SqlParameter("@OverallExperience", SqlDbType.Int) { Value = request.OverallExperience });
                command.Parameters.Add(new SqlParameter("@ServiceQuality", SqlDbType.Int) { Value = request.ServiceQuality });
                command.Parameters.Add(new SqlParameter("@StaffFriendliness", SqlDbType.Int) { Value = request.StaffFriendliness });
                command.Parameters.Add(new SqlParameter("@Cleanliness", SqlDbType.Int) { Value = request.Cleanliness });
                command.Parameters.Add(new SqlParameter("@ValueForMoney", SqlDbType.Int) { Value = request.ValueForMoney });
                AddOptionalParameter(command, "@Experience", SqlDbType.NVarChar, request.Experience, 1000);

                // Execute the stored procedure and read the result
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    var messageIndex = reader.GetOrdinal("Message");
                    return reader.IsDBNull(messageIndex) ? string.Empty : reader.GetString(messageIndex);
                }

                return "Ratings updated successfully";
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        public async Task<object> LocationsCRUDAsync(string action, LocationRequest request)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            
            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_Locations_CRUD", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Action parameter (required)
                command.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 50) { Value = action });

                // Id parameter (for UPDATE, DELETE, GETBYID)
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = request.Id ?? (object)DBNull.Value });

                // Add all other parameters
                AddOptionalParameter(command, "@ServiceProviderId", SqlDbType.Int, request.ServiceProviderId);
                AddOptionalParameter(command, "@LocationName", SqlDbType.NVarChar, request.LocationName, 200);
                AddOptionalParameter(command, "@Manager", SqlDbType.NVarChar, request.Manager, 200);
                AddOptionalParameter(command, "@ManagerId", SqlDbType.Int, request.ManagerId);
                AddOptionalParameter(command, "@Address", SqlDbType.NVarChar, request.Address, 500);
                AddOptionalParameter(command, "@CityId", SqlDbType.NVarChar, request.CityId, 200);
                AddOptionalParameter(command, "@StateId", SqlDbType.NVarChar, request.StateId, 200);
                AddOptionalParameter(command, "@ZipCode", SqlDbType.NVarChar, request.ZipCode, 20);
                AddOptionalParameter(command, "@Phone", SqlDbType.NVarChar, request.Phone, 20);
                AddOptionalParameter(command, "@WMail", SqlDbType.NVarChar, request.WMail, 256);
                AddOptionalParameter(command, "@Status", SqlDbType.NVarChar, request.Status, 50);
                AddOptionalParameter(command, "@AssignedServices", SqlDbType.NVarChar, string.IsNullOrEmpty(request.AssignedServices) ? null : request.AssignedServices, -1);
                AddOptionalParameter(command, "@AssignedEmployees", SqlDbType.NVarChar, string.IsNullOrEmpty(request.AssignedEmployees) ? null : request.AssignedEmployees, -1);

                // Execute the stored procedure
                using var reader = await command.ExecuteReaderAsync();
                
                // Handle different return types based on action
                if (action == "INSERT")
                {
                    if (await reader.ReadAsync())
                    {
                        var newLocationIdIndex = reader.GetOrdinal("NewLocationId");
                        var newLocationId = reader.IsDBNull(newLocationIdIndex) ? (int?)null : Convert.ToInt32(reader.GetValue(newLocationIdIndex));
                        return newLocationId ?? 0;
                    }
                    return 0;
                }
                else if (action == "UPDATE" || action == "DELETE")
                {
                    if (await reader.ReadAsync())
                    {
                        var messageIndex = reader.GetOrdinal("Message");
                        return reader.IsDBNull(messageIndex) ? string.Empty : reader.GetString(messageIndex);
                    }
                    return action == "UPDATE" ? "Updated Successfully" : "Deleted Successfully";
                }
                else if (action == "GETBYID")
                {
                    if (await reader.ReadAsync())
                    {
                        var location = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            location[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        return location;
                    }
                    return null;
                }
                else if (action == "GETALL" || action == "GETBYSERVICEPROVIDERID")
                {
                    var locations = new List<Dictionary<string, object?>>();
                    while (await reader.ReadAsync())
                    {
                        var location = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            location[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        locations.Add(location);
                    }
                    return locations;
                }

                return null;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        public async Task<object> ServicebookGetDetailsAsync(int? servicebookId = null)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            
            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_Servicebook_GetDetails", connection);
                command.CommandType = CommandType.StoredProcedure;

                // ServicebookId parameter (NULL = Get All, NOT NULL = Get by Id)
                command.Parameters.Add(new SqlParameter("@ServicebookId", SqlDbType.Int) { Value = servicebookId ?? (object)DBNull.Value });

                // Execute the stored procedure
                using var reader = await command.ExecuteReaderAsync();
                
                if (servicebookId.HasValue)
                {
                    // Get single record
                    if (await reader.ReadAsync())
                    {
                        var servicebook = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            servicebook[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        return servicebook;
                    }
                    return null;
                }
                else
                {
                    // Get all records
                    var servicebooks = new List<Dictionary<string, object?>>();
                    while (await reader.ReadAsync())
                    {
                        var servicebook = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            servicebook[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        servicebooks.Add(servicebook);
                    }
                    return servicebooks;
                }
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        public async Task<string> BookAppointmentAsync(AppointmentRequest request)
        {
            var existingConnection = Database.GetDbConnection();
            using var connection = new SqlConnection(existingConnection.ConnectionString);
            await connection.OpenAsync();

            try
            {
                // Build XML for pets
                var petsXml = "<Pets>" + string.Join("", request.PetIds.Select(id => $"<Pet><PetId>{id}</PetId></Pet>")) + "</Pets>";

                using var command = new SqlCommand("sp_BookAppointment", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@ServiceProviderId", SqlDbType.Int) { Value = request.ServiceProviderId.HasValue ? request.ServiceProviderId.Value : (object)DBNull.Value });
                command.Parameters.Add(new SqlParameter("@PetParentId", SqlDbType.Int) { Value = request.PetParentId });
                command.Parameters.Add(new SqlParameter("@ServiceId", SqlDbType.Int) { Value = request.ServiceId });
                command.Parameters.Add(new SqlParameter("@AppointmentDate", SqlDbType.Date) { Value = DateOnly.Parse(request.AppointmentDate) });
                command.Parameters.Add(new SqlParameter("@BookingTime", SqlDbType.Time) { Value = TimeSpan.Parse(request.BookingTime) });
                command.Parameters.Add(new SqlParameter("@Pets", SqlDbType.Xml) { Value = petsXml });

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    return reader.IsDBNull(0) ? "Appointment booked successfully" : reader.GetString(0);

                return "Appointment booked successfully";
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
        }

        // ─── PET MEDICAL HELPERS ────────────────────────────────────────────

        public async Task<object> PetMedicalSnapshotCRUDAsync(string action, PetMedicalSnapshotRequest req)
        {
            var conn = Database.GetDbConnection();
            using var connection = new SqlConnection(conn.ConnectionString);
            await connection.OpenAsync();
            try
            {
                using var cmd = new SqlCommand("sp_PetMedicalSnapshot_CRUD", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 20) { Value = action });
                AddOptionalParameter(cmd, "@Id", SqlDbType.Int, req.Id);
                AddOptionalParameter(cmd, "@ServiceProviderId", SqlDbType.Int, req.ServiceProviderId);
                AddOptionalParameter(cmd, "@PetId", SqlDbType.Int, req.PetId);
                AddOptionalParameter(cmd, "@Species", SqlDbType.NVarChar, req.Species, 100);
                AddOptionalParameter(cmd, "@Breed", SqlDbType.NVarChar, req.Breed, 100);
                AddOptionalParameter(cmd, "@DateOfBirth", SqlDbType.Date, req.DateOfBirth != null ? DateOnly.Parse(req.DateOfBirth) : (object?)null);
                AddOptionalParameter(cmd, "@Sex", SqlDbType.NVarChar, req.Sex, 20);
                AddOptionalParameter(cmd, "@Microchipped", SqlDbType.Bit, req.Microchipped);
                AddOptionalParameter(cmd, "@SpayedNeutered", SqlDbType.Bit, req.SpayedNeutered);
                AddOptionalParameter(cmd, "@Allergies", SqlDbType.NVarChar, req.Allergies, -1);
                AddOptionalParameter(cmd, "@ChronicConditions", SqlDbType.NVarChar, req.ChronicConditions, -1);
                AddOptionalParameter(cmd, "@PreferredPharmacy", SqlDbType.NVarChar, req.PreferredPharmacy, 200);
                return await ReadPetMedicalResultAsync(cmd, action);
            }
            catch (SqlException sqlEx) { throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx); }
            catch (Exception ex) { throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex); }
        }

        public async Task<object> PetMedicationsCRUDAsync(string action, PetMedicationRequest req)
        {
            var conn = Database.GetDbConnection();
            using var connection = new SqlConnection(conn.ConnectionString);
            await connection.OpenAsync();
            try
            {
                using var cmd = new SqlCommand("sp_PetMedications_CRUD", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 20) { Value = action });
                AddOptionalParameter(cmd, "@Id", SqlDbType.Int, req.Id);
                AddOptionalParameter(cmd, "@ServiceProviderId", SqlDbType.Int, req.ServiceProviderId);
                AddOptionalParameter(cmd, "@PetId", SqlDbType.Int, req.PetId);
                AddOptionalParameter(cmd, "@MedicationName", SqlDbType.NVarChar, req.MedicationName, 200);
                AddOptionalParameter(cmd, "@Dose", SqlDbType.NVarChar, req.Dose, 100);
                AddOptionalParameter(cmd, "@Frequency", SqlDbType.NVarChar, req.Frequency, 100);
                AddOptionalParameter(cmd, "@PrescribedBy", SqlDbType.NVarChar, req.PrescribedBy, 200);
                AddOptionalParameter(cmd, "@StartDate", SqlDbType.Date, req.StartDate != null ? DateOnly.Parse(req.StartDate) : (object?)null);
                AddOptionalParameter(cmd, "@RefillStatus", SqlDbType.NVarChar, req.RefillStatus, 50);
                return await ReadPetMedicalResultAsync(cmd, action);
            }
            catch (SqlException sqlEx) { throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx); }
            catch (Exception ex) { throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex); }
        }

        public async Task<object> PetDiagnosticsCRUDAsync(int? serviceProviderId, int? petId)
        {
            var conn = Database.GetDbConnection();
            using var connection = new SqlConnection(conn.ConnectionString);
            await connection.OpenAsync();
            try
            {
                using var cmd = new SqlCommand("sp_PetDiagnostics_CRUD", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 20) { Value = "GETALL" });
                AddOptionalParameter(cmd, "@ServiceProviderId", SqlDbType.Int, serviceProviderId);
                AddOptionalParameter(cmd, "@PetId", SqlDbType.Int, petId);
                return await ReadPetMedicalResultAsync(cmd, "GETALL");
            }
            catch (SqlException sqlEx) { throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx); }
            catch (Exception ex) { throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex); }
        }

        public async Task<object> PetDocumentsCRUDAsync(int? serviceProviderId, int? petId)
        {
            var conn = Database.GetDbConnection();
            using var connection = new SqlConnection(conn.ConnectionString);
            await connection.OpenAsync();
            try
            {
                using var cmd = new SqlCommand("sp_PetDocuments_CRUD", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 20) { Value = "GETALL" });
                AddOptionalParameter(cmd, "@ServiceProviderId", SqlDbType.Int, serviceProviderId);
                AddOptionalParameter(cmd, "@PetId", SqlDbType.Int, petId);
                return await ReadPetMedicalResultAsync(cmd, "GETALL");
            }
            catch (SqlException sqlEx) { throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx); }
            catch (Exception ex) { throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex); }
        }

        public async Task<object> PetClinicalSummaryCRUDAsync(int? serviceProviderId, int? petId)
        {
            var conn = Database.GetDbConnection();
            using var connection = new SqlConnection(conn.ConnectionString);
            await connection.OpenAsync();
            try
            {
                using var cmd = new SqlCommand("sp_PetClinicalSummary_CRUD", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 20) { Value = "GETALL" });
                AddOptionalParameter(cmd, "@ServiceProviderId", SqlDbType.Int, serviceProviderId);
                AddOptionalParameter(cmd, "@PetId", SqlDbType.Int, petId);
                return await ReadPetMedicalResultAsync(cmd, "GETALL");
            }
            catch (SqlException sqlEx) { throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx); }
            catch (Exception ex) { throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex); }
        }

        public async Task<object> PetRemindersCRUDAsync(int? serviceProviderId, int? petId)
        {
            var conn = Database.GetDbConnection();
            using var connection = new SqlConnection(conn.ConnectionString);
            await connection.OpenAsync();
            try
            {
                using var cmd = new SqlCommand("sp_PetReminders_CRUD", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 20) { Value = "GETALL" });
                AddOptionalParameter(cmd, "@ServiceProviderId", SqlDbType.Int, serviceProviderId);
                AddOptionalParameter(cmd, "@PetId", SqlDbType.Int, petId);
                return await ReadPetMedicalResultAsync(cmd, "GETALL");
            }
            catch (SqlException sqlEx) { throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx); }
            catch (Exception ex) { throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex); }
        }

        public async Task<object> PetPermissionsCRUDAsync(string action, PetPermissionRequest req)
        {
            var conn = Database.GetDbConnection();
            using var connection = new SqlConnection(conn.ConnectionString);
            await connection.OpenAsync();
            try
            {
                using var cmd = new SqlCommand("sp_PetPermissions_CRUD", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 20) { Value = action });
                AddOptionalParameter(cmd, "@Id", SqlDbType.Int, req.Id);
                AddOptionalParameter(cmd, "@ServiceProviderId", SqlDbType.Int, req.ServiceProviderId);
                AddOptionalParameter(cmd, "@PetId", SqlDbType.Int, req.PetId);
                AddOptionalParameter(cmd, "@UserName", SqlDbType.NVarChar, req.UserName, 200);
                AddOptionalParameter(cmd, "@Role", SqlDbType.NVarChar, req.Role, 100);
                AddOptionalParameter(cmd, "@AccessLevel", SqlDbType.NVarChar, req.AccessLevel, 50);
                AddOptionalParameter(cmd, "@IsActive", SqlDbType.Bit, req.IsActive);
                return await ReadPetMedicalResultAsync(cmd, action);
            }
            catch (SqlException sqlEx) { throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx); }
            catch (Exception ex) { throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex); }
        }

        public async Task<object> PetVaccinationsCRUDAsync(int? petId)
        {
            var conn = Database.GetDbConnection();
            using var connection = new SqlConnection(conn.ConnectionString);
            await connection.OpenAsync();
            try
            {
                using var cmd = new SqlCommand("sp_PetVaccinations_CRUD", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 20) { Value = "GETALL" });
                AddOptionalParameter(cmd, "@ServiceProviderId", SqlDbType.Int, (object?)null);
                AddOptionalParameter(cmd, "@PetId", SqlDbType.Int, petId);
                return await ReadPetMedicalResultAsync(cmd, "GETALL");
            }
            catch (SqlException sqlEx) { throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx); }
            catch (Exception ex) { throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex); }
        }

        public async Task<object> GetFullPetDashboardAsync(int serviceProviderId, int petId)
        {
            var conn = Database.GetDbConnection();
            using var connection = new SqlConnection(conn.ConnectionString);
            await connection.OpenAsync();
            try
            {
                using var cmd = new SqlCommand("sp_GetFullPetDashboard", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ServiceProviderId", SqlDbType.Int) { Value = serviceProviderId });
                cmd.Parameters.Add(new SqlParameter("@PetId", SqlDbType.Int) { Value = petId });

                using var reader = await cmd.ExecuteReaderAsync();
                var dashboard = new Dictionary<string, object?>();
                string[] keys = { "medicalSnapshot", "medications", "diagnostics", "documents", "reminders", "vaccinations" };
                int index = 0;
                do
                {
                    var list = new List<Dictionary<string, object?>>();
                    while (await reader.ReadAsync())
                    {
                        var row = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                            row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        list.Add(row);
                    }
                    if (index < keys.Length) dashboard[keys[index]] = list;
                    index++;
                } while (await reader.NextResultAsync());

                return dashboard;
            }
            catch (SqlException sqlEx) { throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx); }
            catch (Exception ex) { throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex); }
        }

        private async Task<object> ReadPetMedicalResultAsync(SqlCommand cmd, string action)
        {
            using var reader = await cmd.ExecuteReaderAsync();
            if (action == "INSERT")
            {
                if (await reader.ReadAsync())
                    return reader.IsDBNull(0) ? 0 : Convert.ToInt32(reader.GetValue(0));
                return 0;
            }
            else if (action == "DELETE" || action == "UPDATE")
            {
                return action == "DELETE" ? "Deleted successfully" : "Updated successfully";
            }
            else if (action == "GETBYID")
            {
                if (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object?>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    return row;
                }
                return null;
            }
            else // GETALL
            {
                var list = new List<Dictionary<string, object?>>();
                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object?>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    list.Add(row);
                }
                return list;
            }
        }

        public async Task<object> GetAllServiceProvidersAsync()
        {
            var existingConnection = Database.GetDbConnection();
            using var connection = new SqlConnection(existingConnection.ConnectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_ServiceProviders_Get", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = DBNull.Value });

                using var reader = await command.ExecuteReaderAsync();
                var list = new List<Dictionary<string, object?>>();
                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object?>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    list.Add(row);
                }
                return list;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
        }

        public async Task<object?> GetServiceProviderByIdAsync(int id)
        {
            var existingConnection = Database.GetDbConnection();
            using var connection = new SqlConnection(existingConnection.ConnectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_ServiceProviders_Get", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object?>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    return row;
                }
                return null;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
        }

        public async Task<object> GetPetParentsByServiceProviderIdAsync(int serviceProviderId)
        {
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_PetParents_Get", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = DBNull.Value });
                command.Parameters.Add(new SqlParameter("@ServiceProviderId", SqlDbType.Int) { Value = serviceProviderId });

                using var reader = await command.ExecuteReaderAsync();
                var petParents = new List<Dictionary<string, object?>>();
                while (await reader.ReadAsync())
                {
                    var petParent = new Dictionary<string, object?>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var fieldName = reader.GetName(i);
                        petParent[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    }
                    petParents.Add(petParent);
                }
                return petParents;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
        }

        public async Task<object?> PetParentLoginAsync(PetParentLoginRequest request)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            
            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_PetParents_Login", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Required parameters
                command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 256) { Value = request.Email ?? (object)DBNull.Value });
                command.Parameters.Add(new SqlParameter("@Password", SqlDbType.VarChar, 30) { Value = request.Password ?? (object)DBNull.Value });

                // Execute the stored procedure
                using var reader = await command.ExecuteReaderAsync();
                
                // Check if a record was returned (successful login)
                // The stored procedure returns rows on success, or no rows on failure (WHERE 1=0)
                if (await reader.ReadAsync())
                {
                    // Login successful - return full record
                    var petParent = new Dictionary<string, object?>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var fieldName = reader.GetName(i);
                        petParent[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    }
                    return petParent;
                }
                
                // No record returned - login failed
                return null;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        public async Task<Dictionary<string, object?>> PetParentPetInsertAsync(PetParentPetInsertRequest request)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;
            
            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_PetParents_Pets_Insert", connection);
                command.CommandType = CommandType.StoredProcedure;

                // PetParent parameters
                command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 256) { Value = request.Email ?? (object)DBNull.Value });
                command.Parameters.Add(new SqlParameter("@Password", SqlDbType.VarChar, 30) { Value = request.Password ?? (object)DBNull.Value });
                command.Parameters.Add(new SqlParameter("@FullName", SqlDbType.NVarChar, 200) { Value = request.FullName ?? (object)DBNull.Value });
                command.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int) { Value = request.Status });

                // Pet parameters
                command.Parameters.Add(new SqlParameter("@PetName", SqlDbType.NVarChar, 100) { Value = request.PetName ?? (object)DBNull.Value });
                command.Parameters.Add(new SqlParameter("@Species", SqlDbType.NVarChar, 100) { Value = request.Species ?? (object)DBNull.Value });
                AddOptionalParameter(command, "@Breed", SqlDbType.NVarChar, request.Breed, 100);
                AddOptionalParameter(command, "@Age", SqlDbType.Int, request.Age);
                AddOptionalParameter(command, "@Sex", SqlDbType.NVarChar, request.Sex, 20);
                AddOptionalParameter(command, "@Microchipped", SqlDbType.Bit, request.Microchipped);
                AddOptionalParameter(command, "@Spayed", SqlDbType.Bit, request.Spayed);
                AddOptionalParameter(command, "@Allergies", SqlDbType.NVarChar, request.Allergies, -1);
                AddOptionalParameter(command, "@Vaccination", SqlDbType.NVarChar, request.Vaccination, -1);
                AddOptionalParameter(command, "@Medications", SqlDbType.NVarChar, request.Medications, -1);
                AddOptionalParameter(command, "@LastVisit", SqlDbType.Date, request.LastVisit);
                AddOptionalParameter(command, "@ReasonForVisit", SqlDbType.NVarChar, request.ReasonForVisit, 500);
                AddOptionalParameter(command, "@MedicalHistory", SqlDbType.NVarChar, request.MedicalHistory, -1);
                AddOptionalParameter(command, "@MedicalFileName", SqlDbType.NVarChar, request.MedicalFileName, 500);
                command.Parameters.Add(new SqlParameter("@PetStatus", SqlDbType.Int) { Value = request.PetStatus });

                // Execute the stored procedure
                using var reader = await command.ExecuteReaderAsync();
                
                // Read the result (PetParentId and PetId)
                if (await reader.ReadAsync())
                {
                    var result = new Dictionary<string, object?>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var fieldName = reader.GetName(i);
                        result[fieldName] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    }
                    return result;
                }
                
                // Should not happen if stored procedure executes successfully
                throw new InvalidOperationException("Stored procedure executed but no result was returned");
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Database error: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        public async Task<ProviderLoginResponse?> ProviderLoginAsync(string email, string password)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;

            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                // Encode the password using Base64 (as the database stores Base64 encoded passwords)
                string passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));

                using var command = new SqlCommand("sp_Provider_Login", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Required parameters
                command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 256) { Value = email ?? (object)DBNull.Value });
                command.Parameters.Add(new SqlParameter("@PasswordHash", SqlDbType.NVarChar, -1) { Value = passwordHash ?? (object)DBNull.Value });

                // Execute the stored procedure
                using var reader = await command.ExecuteReaderAsync();

                // Check if a record was returned (successful login)
                if (await reader.ReadAsync())
                {
                    // Login successful - return provider data
                    var provider = new ProviderLoginResponse();
                    
                    // Get Id (column name is "Id")
                    try
                    {
                        var idIndex = reader.GetOrdinal("Id");
                        provider.Id = reader.GetInt32(idIndex);
                    }
                    catch { }

                    // Get Email (column name is "Email")
                    try
                    {
                        var emailIndex = reader.GetOrdinal("Email");
                        provider.Email = reader.GetString(emailIndex);
                    }
                    catch { }

                    // Get Status (column name is "Status", not "AccountStatus")
                    try
                    {
                        var statusIndex = reader.GetOrdinal("Status");
                        provider.AccountStatus = reader.GetString(statusIndex);
                    }
                    catch { }

                    return provider;
                }

                // No record returned - login failed
                return null;
            }
            catch (SqlException sqlEx)
            {
                // Check if this is the "Invalid email or password" error from the stored procedure
                if (sqlEx.Message.Contains("Invalid email or password"))
                {
                    // Return null to indicate login failed (not an exception)
                    return null;
                }
                throw new InvalidOperationException($"Database error calling sp_Provider_Login: {sqlEx.Message} (Error Number: {sqlEx.Number}, Line: {sqlEx.LineNumber})", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure sp_Provider_Login: {ex.Message}", ex);
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }

        public async Task<object?> ProviderLoginStoredProcedureAsync(string email, string passwordHash)
        {
            // Get connection string from the existing connection
            var existingConnection = Database.GetDbConnection();
            var connectionString = existingConnection.ConnectionString;

            // Create a completely NEW independent connection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new SqlCommand("sp_Provider_Login", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Required parameters
                command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 256) { Value = email ?? (object)DBNull.Value });
                command.Parameters.Add(new SqlParameter("@PasswordHash", SqlDbType.NVarChar, 500) { Value = passwordHash ?? (object)DBNull.Value });

                // Execute the stored procedure
                using var reader = await command.ExecuteReaderAsync();

                // Check if a record was returned (successful login)
                if (await reader.ReadAsync())
                {
                    // Check the LoginSource to determine the response type
                    var loginSource = reader.GetOrdinal("LoginSource");
                    var source = reader.IsDBNull(loginSource) ? "" : reader.GetString(loginSource);

                    if (source == "FINAL")
                    {
                        // Return FINAL table data
                        var provider = new ProviderLoginFinalResponse
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ProviderId = reader.IsDBNull(reader.GetOrdinal("ProviderId")) ? null : reader.GetInt32(reader.GetOrdinal("ProviderId")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            BusinessName = reader.GetString(reader.GetOrdinal("BusinessName")),
                            OwnerName = reader.GetString(reader.GetOrdinal("OwnerName")),
                            Status = reader.GetString(reader.GetOrdinal("Status")),
                            IsApproved = reader.GetBoolean(reader.GetOrdinal("IsApproved")),
                            IsLive = reader.GetBoolean(reader.GetOrdinal("IsLive")),
                            UserId = reader.IsDBNull(reader.GetOrdinal("UserId")) ? null : reader.GetInt32(reader.GetOrdinal("UserId")),
                            LoginSource = "FINAL"
                        };
                        return provider;
                    }
                    else if (source == "TEMP")
                    {
                        // Return TEMP table data
                        var provider = new ProviderLoginTempResponse
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ProviderId = reader.IsDBNull(reader.GetOrdinal("ProviderId")) ? null : reader.GetInt32(reader.GetOrdinal("ProviderId")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            BusinessName = reader.GetString(reader.GetOrdinal("BusinessName")),
                            OwnerName = reader.GetString(reader.GetOrdinal("OwnerName")),
                            Status = reader.GetString(reader.GetOrdinal("Status")),
                            CurrentStep = reader.IsDBNull(reader.GetOrdinal("CurrentStep")) ? null : reader.GetInt32(reader.GetOrdinal("CurrentStep")),
                            ProgressPercentage = reader.IsDBNull(reader.GetOrdinal("ProgressPercentage")) ? null : reader.GetInt32(reader.GetOrdinal("ProgressPercentage")),
                            LoginSource = "TEMP"
                        };
                        return provider;
                    }
                }

                // No record returned - login failed, return error response
                return new ProviderLoginErrorResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }
            catch (SqlException sqlEx)
            {
                // Return error response on SQL exception
                return new ProviderLoginErrorResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }
            catch (Exception ex)
            {
                // Return error response on general exception
                return new ProviderLoginErrorResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }
            finally
            {
                // Connection will be disposed automatically by using statement
            }
        }
    }
}

