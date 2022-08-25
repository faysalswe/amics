using Aims.Core.Models;
using Aims.PartMaster.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;


namespace Aims.Core.Services
{
    public interface IContractService
    {
        List<LstContracts> LoadContracts(string contract, string project);
        List<LstProjects> LoadProjects(string contractId);
        LstMessage UpdateContracts(List<LstContracts> lstContracts);
        LstMessage UpdateProjects(List<LstProjects> lstProjects);
    }
    public class ContractService: IContractService
    {
        private readonly AmicsDbContext _amicsDbContext;
        private ILogger<ContractService> _logger;
        Utility util = new Utility();
        public ContractService(AmicsDbContext aimsDbContext, ILogger<ContractService> logger)
        {
            _amicsDbContext = aimsDbContext;
            _logger = logger;
        }

        /// <summary>
        /// API Service to load contracts from the table list_contracts and list_projects
        /// 
        /// </summary>
        /// <param name="contract">contract</param>        
        /// <param name="project">project</param>  
        public List<LstContracts> LoadContracts(string contract, string project)
        {
            List<LstContracts> lstContracts = new List<LstContracts>();

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_loadcontract";
                    conn.Open();

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@contract", contract));
                    sqlCommand.Parameters.Add(new SqlParameter("@project", project));                    
                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        LstContracts contracts = new LstContracts();
                        contracts.Id = dataReader["id"].ToString();
                        contracts.ContractNo = dataReader["contractnum"].ToString();
                        contracts.ContractName = dataReader["description"].ToString();

                        if ((dataReader["markup1"] != DBNull.Value) || (dataReader["markup1"].ToString() != ""))
                            contracts.Markup1 = String.Format("{0:0." + "0000" + "}", dataReader["markup1"]);

                        if ((dataReader["markup2"] != DBNull.Value) || (dataReader["markup2"].ToString() != ""))
                            contracts.Markup2 = String.Format("{0:0." + "0000" + "}", dataReader["markup2"]);

                        lstContracts.Add(contracts);
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    sqlCommand.Dispose();
                    conn.Close();
                }
            }
            return lstContracts.ToList();
        }

        /// <summary>
        /// API Service to bind project details in the grid for selected contract         
        /// </summary>
        /// <param name="contractId">contractId</param>                
        public List<LstProjects> LoadProjects(string contractId)
        {
            List<LstProjects> lstProjects = new List<LstProjects>();

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_load_projects";
                    conn.Open();

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@contractid", string.IsNullOrWhiteSpace(contractId) ? string.Empty : contractId));
                    
                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        LstProjects projects = new LstProjects();
                        projects.Id = dataReader["id"].ToString();
                        projects.ContractId = dataReader["contractid"].ToString();
                        projects.Project = dataReader["project"].ToString();
                        projects.Name = dataReader["name"].ToString();                       
                        lstProjects.Add(projects);                       
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    sqlCommand.Dispose();
                    conn.Close();
                }
            }
            return lstProjects.ToList();
        }

        /// <summary>
        /// API Service to insert/update/delete contract details in the list_contracts table     
        /// </summary>
        /// <param name="lstContracts">lstContracts</param>            
        public LstMessage UpdateContracts(List<LstContracts> lstContracts)
        {
            List<LstProjects> lstProjects = new List<LstProjects>();
            var contractNum = ""; var contractId = "";

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            {
                for (int i = 0; i < lstContracts.Count; i++)
                {
                    using (var command = _amicsDbContext.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = "amics_sp_api_maintain_contracts";
                        command.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        try
                        {
                            contractNum = lstContracts[0].ContractNo;
                            command.Parameters.Add(new SqlParameter("@actionflag", lstContracts[i].ActionFlag));
                            if (lstContracts[i].Id != null)
                                command.Parameters.Add(new SqlParameter("@id", lstContracts[i].Id));

                            command.Parameters.Add(new SqlParameter("@contractnum", lstContracts[i].ContractNo));
                            command.Parameters.Add(new SqlParameter("@description", lstContracts[i].ContractName));
                            command.Parameters.Add(new SqlParameter("@markup1", lstContracts[i].Markup1));
                            command.Parameters.Add(new SqlParameter("@markup2", lstContracts[i].Markup2));                            
                            command.Parameters.Add(new SqlParameter("@createdby", lstContracts[i].UserName));                            
                            command.ExecuteNonQuery();
                            command.Dispose();

                        }
                        catch (Exception ex)
                        {                          
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }

                using (var command = _amicsDbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select top 1 id from list_contracts where contractnum = '" + contractNum + "'";
                    command.CommandType = CommandType.Text;
                    conn.Open();
                    var dataReader = command.ExecuteReader();

                    if (dataReader.Read())
                        contractId = dataReader["id"].ToString();

                    dataReader.Close();
                    command.Dispose();
                }
                conn.Close();
            }
            return new LstMessage() { Message = "Successfully Saved" };

        }

        /// <summary>
        /// API Service to insert/update/delete project details in the list_projects table        
        /// </summary>
        /// <param name="lstProjects">lstProjects</param>                
        /// <param name="userName">userName</param>                
        public LstMessage UpdateProjects(List<LstProjects> lstProjects)
        {
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            {
                for (int i = 0; i < lstProjects.Count; i++)
                {
                    using (var command = _amicsDbContext.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = "amics_sp_api_maintain_projects";
                        command.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        try
                        {
                            command.Parameters.Add(new SqlParameter("@actionFlag", lstProjects[i].ActionFlag));
                            command.Parameters.Add(new SqlParameter("@id", lstProjects[i].Id));
                            command.Parameters.Add(new SqlParameter("@project", lstProjects[i].Project));
                            command.Parameters.Add(new SqlParameter("@name", lstProjects[i].Name));
                            command.Parameters.Add(new SqlParameter("@createdby", lstProjects[i].UserName));
                            command.Parameters.Add(new SqlParameter("@contractid", lstProjects[i].ContractId));
                            command.ExecuteNonQuery();
                            command.Dispose();

                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
            return new LstMessage() { Message = "Successfully Saved" };
        }
    }
}
