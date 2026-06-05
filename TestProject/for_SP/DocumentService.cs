using FcTrx.Model.ViewModels;
using FcTrx.Web.Classes;
using Microsoft.Data.SqlClient;
using System.Data;
using static FcTrx.Model.ViewModels.DocumentPagedDto;

public class DocumentService
{
    private readonly string _connectionString;
    public DocumentService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<DocumentPagedDataDto>> GetDocumentPagedDataAsync(int page = 1, int pageSize = 20, DocumentSearchParam searchParam = null) // DateTime? fromDate = null, DateTime? toDate = null, string branchId = null, string entryBy = null, int? categoryId = null, int? tagId = null, string tagValue = null)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            var resultList = new List<DocumentPagedDataDto>();
            using (SqlCommand cmd = new SqlCommand("[dbo].[GetDocumentPagedData_v2]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@fromDate", (object?)searchParam.FromDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@toDate", (object?)searchParam.ToDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@branchId", (object?)searchParam.BranchId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@loggedInEmp", (object?)searchParam.EntryBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@categoryId", (object?)searchParam.CategoryId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@tagId", (object?)searchParam.TagId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@tagValue", (object?)searchParam.TagValue ?? DBNull.Value);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var document = new DocumentPagedDataDto
                        {
                            Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                            EntryTime = reader["EntryTime"].ToString(),
                            Description = reader["Description"]?.ToString(),
                            BranchName = reader["BranchName"]?.ToString(),
                            CategoryName = reader["CategoryName"]?.ToString(),
                            TagValues = reader["TagValues"]?.ToString(),
                            StatusName = reader["StatusName"]?.ToString(),
                            TotalCount = reader["TotalCount"] != DBNull.Value ? (int)reader["TotalCount"] : 0
                        };
                        resultList.Add(document);
                    }
                }
            }
            return resultList;
        }
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
    {
        var summary = new DashboardSummaryDto();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            using (SqlCommand cmd = new SqlCommand("dbo.sp_GetTableCounts", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        summary.TotalCategories = reader["TotalCategories"] != DBNull.Value ? Convert.ToInt32(reader["TotalCategories"]) : 0;
                        summary.ActiveCategories = reader["ActiveCategories"] != DBNull.Value ? Convert.ToInt32(reader["ActiveCategories"]) : 0;
                        summary.InactiveCategories = reader["InactiveCategories"] != DBNull.Value ? Convert.ToInt32(reader["InactiveCategories"]) : 0;
                        summary.TotalCategoryBranches = reader["TotalCategoryBranches"] != DBNull.Value ? Convert.ToInt32(reader["TotalCategoryBranches"]) : 0;
                        summary.TotalUploadedFiles = reader["TotalUploadedFiles"] != DBNull.Value ? Convert.ToInt32(reader["TotalUploadedFiles"]) : 0;
                        summary.TotalUserRoles = reader["TotalUserRoles"] != DBNull.Value ? Convert.ToInt32(reader["TotalUserRoles"]) : 0;
                    }
                }
            }
        }
        return summary;
    }
}