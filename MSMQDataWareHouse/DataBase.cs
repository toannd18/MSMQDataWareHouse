using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MSMQFlightDataFormat;

namespace MSMQDatabase
{
	class FlightDatabase
	{
		public static SqlConnection GetDBConnection(string connString)
		{
			return new SqlConnection(connString);
		}

		public static bool QueryData(SqlConnection connnection, string idQuery)
		{
			bool result = false;
			string queryCmd = $"SELECT * FROM AHT_FlightInformation WHERE id = '{idQuery}'";
			using (SqlCommand cmd = new SqlCommand(queryCmd, connnection))
			{
				SqlDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					result = true;
				}
				reader.Close();
			}
			return result;
		}

		public static void AddData(SqlConnection connection, AHT_FlightInformation flightInformation)
		{
			string insertCmd = "Insert into AHT_FlightInformation (Id, ActionName, Adi, LineCode, Number, ScheduledDate, Domesticintcode, City, CityName, Aircraft, PaxCount, Schedule, Estimated, Actual, Gate, Status, Remark, RowFrom, Rowto, Belt, GateStart, GateEnd, CounterStart, CounterEnd, ClaimStart, ClaimEnd, Mcdt, Mcat, Nopa, Nopc, Totalpax, Transit24, Potablewater, Aerobridgefrom, Aerobridgeto, Gpu90From, Gpu90To, Gpu180From, Gpu180To, Acu45From, Acu45To, Qualifier, Amsflightid, Amslinkedflightid  )  values (@id, @actionname, @adi, @linecode, @number, @scheduleddate, @domesticintcode, @city, @cityname, @aircraft, @paxcount, @schedule, @estimated, @actual, @gate,  @status, @remark, @rowfrom, @rowto, @belt, @gatestart, @gateend, @counterstart, @counterend, @claimstart, @claimend, @mcdt, @mcat, @nopa, @nopc, @totalpax, @transit24, @potablewater, @aerobridgefrom, @aerobridgeto, @gpu90from, @gpu90to, @gpu180from, @gpu180to, @acu45from, @acu45to, @qualifier, @amsflightid, @amslinkedflightid ) ";
			SqlCommand sqlCmd = new SqlCommand(insertCmd, connection);
			sqlCmd.Parameters.AddWithValue("@id", flightInformation.Id ?? "");
			sqlCmd.Parameters.AddWithValue("@actionname", flightInformation.ActionName ?? "");
			sqlCmd.Parameters.AddWithValue("@adi", flightInformation.Adi ?? "");
			sqlCmd.Parameters.AddWithValue("@linecode", flightInformation.LineCode ?? "");
			sqlCmd.Parameters.AddWithValue("@number", flightInformation.Number ?? "");
			sqlCmd.Parameters.AddWithValue("@scheduleddate", flightInformation.ScheduledDate ?? "");
			sqlCmd.Parameters.AddWithValue("@domesticintcode", flightInformation.Domesticintcode ?? "");
			sqlCmd.Parameters.AddWithValue("@city", flightInformation.City ?? "");
			sqlCmd.Parameters.AddWithValue("@cityname", flightInformation.CityName ?? "");
			sqlCmd.Parameters.AddWithValue("@aircraft", flightInformation.Aircraft ?? "");
			sqlCmd.Parameters.AddWithValue("@paxcount", flightInformation.PaxCount ?? "");
			SqlParameterCollection parameters = sqlCmd.Parameters;
			DateTime? schedule = flightInformation.Schedule;
			parameters.AddWithValue("@schedule", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters2 = sqlCmd.Parameters;
			schedule = flightInformation.Estimated;
			parameters2.AddWithValue("@estimated", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters3 = sqlCmd.Parameters;
			schedule = flightInformation.Actual;
			parameters3.AddWithValue("@actual", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			sqlCmd.Parameters.AddWithValue("@gate", flightInformation.Gate ?? "");
			sqlCmd.Parameters.AddWithValue("@status", flightInformation.Status ?? "");
			sqlCmd.Parameters.AddWithValue("@remark", flightInformation.Remark ?? "");
			sqlCmd.Parameters.AddWithValue("@rowfrom", flightInformation.RowFrom ?? "");
			sqlCmd.Parameters.AddWithValue("@rowto", flightInformation.RowTo ?? "");
			sqlCmd.Parameters.AddWithValue("@belt", flightInformation.Belt ?? "");
			SqlParameterCollection parameters4 = sqlCmd.Parameters;
			schedule = flightInformation.GateStart;
			parameters4.AddWithValue("@gatestart", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters5 = sqlCmd.Parameters;
			schedule = flightInformation.GateEnd;
			parameters5.AddWithValue("@gateend", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters6 = sqlCmd.Parameters;
			schedule = flightInformation.CounterStart;
			parameters6.AddWithValue("@counterstart", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters7 = sqlCmd.Parameters;
			schedule = flightInformation.CounterEnd;
			parameters7.AddWithValue("@counterend", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters8 = sqlCmd.Parameters;
			schedule = flightInformation.ClaimStart;
			parameters8.AddWithValue("@claimstart", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters9 = sqlCmd.Parameters;
			schedule = flightInformation.ClaimEnd;
			parameters9.AddWithValue("@claimend", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters10 = sqlCmd.Parameters;
			schedule = flightInformation.Mcdt;
			parameters10.AddWithValue("@mcdt", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters11 = sqlCmd.Parameters;
			schedule = flightInformation.Mcat;
			parameters11.AddWithValue("@mcat", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			sqlCmd.Parameters.AddWithValue("@nopa", flightInformation.Nopa ?? "");
			sqlCmd.Parameters.AddWithValue("@nopc", flightInformation.Nopc ?? "");
			sqlCmd.Parameters.AddWithValue("@totalpax", flightInformation.Totalpax ?? "");
			sqlCmd.Parameters.AddWithValue("@potablewater", flightInformation.Potablewater ?? "");
			SqlParameterCollection parameters12 = sqlCmd.Parameters;
			schedule = flightInformation.AerobridgeFrom;
			parameters12.AddWithValue("@aerobridgefrom", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters13 = sqlCmd.Parameters;
			schedule = flightInformation.AerobridgeTo;
			parameters13.AddWithValue("@aerobridgeto", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters14 = sqlCmd.Parameters;
			schedule = flightInformation.Gpu90From;
			parameters14.AddWithValue("@gpu90from", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters15 = sqlCmd.Parameters;
			schedule = flightInformation.Gpu90To;
			parameters15.AddWithValue("@gpu90to", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters16 = sqlCmd.Parameters;
			schedule = flightInformation.Gpu180From;
			parameters16.AddWithValue("@gpu180from", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters17 = sqlCmd.Parameters;
			schedule = flightInformation.Gpu180To;
			parameters17.AddWithValue("@gpu180to", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters18 = sqlCmd.Parameters;
			schedule = flightInformation.Acu45From;
			parameters18.AddWithValue("@acu45from", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters19 = sqlCmd.Parameters;
			schedule = flightInformation.Acu45To;
			parameters19.AddWithValue("@acu45to", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			sqlCmd.Parameters.AddWithValue("@qualifier", flightInformation.Qualifier ?? "");
			sqlCmd.Parameters.AddWithValue("@amsflightid", flightInformation.Amsflightid ?? "");
			sqlCmd.Parameters.AddWithValue("@amslinkedflightid", flightInformation.Amslinkedflightid ?? "");
			sqlCmd.CommandType = CommandType.Text;
			int i = sqlCmd.ExecuteNonQuery();
		}

		public static void UpdateData(SqlConnection connection, AHT_FlightInformation flightInformation)
		{
			string updateCmd = "UPDATE AHT_FlightInformation SET ActionName=@actionname,Adi=@adi,LineCode=@linecode,Number=@number,ScheduledDate=@scheduleddate";
			SqlCommand sqlCmd = connection.CreateCommand();
			if (flightInformation.Domesticintcode != null)
			{
				updateCmd += ",Domesticintcode=@domesticintcode";
				sqlCmd.Parameters.AddWithValue("@domesticintcode", flightInformation.Domesticintcode ?? "");
			}
			if (flightInformation.City != null)
			{
				updateCmd += ",City=@city";
				sqlCmd.Parameters.AddWithValue("@city", flightInformation.City ?? "");
			}
			if (flightInformation.CityName != null)
			{
				updateCmd += ",CityName=@cityname";
				sqlCmd.Parameters.AddWithValue("@cityname", flightInformation.CityName ?? "");
			}
			if (flightInformation.Aircraft != null)
			{
				updateCmd += ",Aircraft=@aircraft";
				sqlCmd.Parameters.AddWithValue("@aircraft", flightInformation.Aircraft ?? "");
			}
			if (flightInformation.PaxCount != null)
			{
				updateCmd += ",PaxCount=@paxcount";
				sqlCmd.Parameters.AddWithValue("@paxcount", flightInformation.PaxCount ?? "");
			}
			if (flightInformation.Schedule.HasValue)
			{
				updateCmd += ",Schedule=@schedule";
				SqlParameterCollection parameters = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.Schedule;
				parameters.AddWithValue("@schedule", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.Actual.HasValue)
			{
				updateCmd += ",Actual=@actual";
				SqlParameterCollection parameters2 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.Actual;
				parameters2.AddWithValue("@actual", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.Estimated.HasValue)
			{
				updateCmd += ",Estimated=@estimated";
				SqlParameterCollection parameters3 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.Estimated;
				parameters3.AddWithValue("@estimated", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.Gate != null)
			{
				updateCmd += ",Gate=@gate";
				sqlCmd.Parameters.AddWithValue("@gate", flightInformation.Gate ?? "");
			}
			if (flightInformation.Status != null)
			{
				updateCmd += ",Status=@status";
				sqlCmd.Parameters.AddWithValue("@status", flightInformation.Status ?? "");
			}
			if (flightInformation.Remark != null)
			{
				updateCmd += ",Remark=@remark";
				sqlCmd.Parameters.AddWithValue("@remark", flightInformation.Remark ?? "");
			}
			if (flightInformation.RowFrom != null)
			{
				updateCmd += ",RowFrom=@rowfrom,RowTo=@rowTo";
				sqlCmd.Parameters.AddWithValue("@rowfrom", flightInformation.RowFrom ?? "");
				sqlCmd.Parameters.AddWithValue("@rowto", flightInformation.RowTo ?? "");
			}
			if (flightInformation.RowTo != null)
			{
				updateCmd += ",RowFrom=@rowfrom,RowTo=@rowTo";
				sqlCmd.Parameters.AddWithValue("@rowfrom", flightInformation.RowFrom ?? "");
				sqlCmd.Parameters.AddWithValue("@rowto", flightInformation.RowTo ?? "");
			}
			if (flightInformation.Belt != null)
			{
				updateCmd += ",Belt=@belt";
				sqlCmd.Parameters.AddWithValue("@belt", flightInformation.Belt ?? "");
			}
			if (flightInformation.GateStart.HasValue)
			{
				updateCmd += ",GateStart=@gatestart";
				SqlParameterCollection parameters4 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.GateStart;
				parameters4.AddWithValue("@gatestart", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.GateEnd.HasValue)
			{
				updateCmd += ",GateEnd=@gateend";
				SqlParameterCollection parameters5 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.GateEnd;
				parameters5.AddWithValue("@gateend", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.CounterStart.HasValue)
			{
				updateCmd += ",CounterStart=@counterstart";
				SqlParameterCollection parameters6 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.CounterStart;
				parameters6.AddWithValue("@counterstart", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.CounterEnd.HasValue)
			{
				updateCmd += ",CounterEnd=@counterend";
				SqlParameterCollection parameters7 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.CounterEnd;
				parameters7.AddWithValue("@counterend", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.ClaimStart.HasValue)
			{
				updateCmd += ",ClaimStart=@claimstart";
				SqlParameterCollection parameters8 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.ClaimStart;
				parameters8.AddWithValue("@claimstart", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.ClaimEnd.HasValue)
			{
				updateCmd += ",ClaimEnd=@claimend";
				SqlParameterCollection parameters9 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.ClaimEnd;
				parameters9.AddWithValue("@claimend", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.Mcdt.HasValue)
			{
				updateCmd += ",Mcdt=@mcdt";
				SqlParameterCollection parameters10 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.Mcdt;
				parameters10.AddWithValue("@mcdt", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.Mcat.HasValue)
			{
				updateCmd += ",Mcat=@mcat";
				SqlParameterCollection parameters11 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.Mcat;
				parameters11.AddWithValue("@mcat", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.Nopa != null)
			{
				updateCmd += ",Nopa=@nopa";
				sqlCmd.Parameters.AddWithValue("@nopa", flightInformation.Nopa ?? "");
			}
			if (flightInformation.Nopc != null)
			{
				updateCmd += ",Nopc=@nopc";
				sqlCmd.Parameters.AddWithValue("@nopc", flightInformation.Nopc ?? "");
			}
			if (flightInformation.Totalpax != null)
			{
				updateCmd += ",Totalpax=@totalpax";
				sqlCmd.Parameters.AddWithValue("@totalpax", flightInformation.Totalpax ?? "");
			}
			if (flightInformation.Transit24 != null)
			{
				updateCmd += ",Transit24=@transit24";
				sqlCmd.Parameters.AddWithValue("@transit24", flightInformation.Transit24 ?? "");
			}
			if (flightInformation.Potablewater != null)
			{
				updateCmd += ",Potablewater=@potablewater";
				sqlCmd.Parameters.AddWithValue("@potablewater", flightInformation.Potablewater ?? "");
			}
			if (flightInformation.AerobridgeFrom.HasValue)
			{
				updateCmd += ",AerobridgeFrom=@aerobridgefrom";
				SqlParameterCollection parameters12 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.AerobridgeFrom;
				parameters12.AddWithValue("@aerobridgefrom", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.AerobridgeTo.HasValue)
			{
				updateCmd += ",AerobridgeTo=@aerobridgeto";
				SqlParameterCollection parameters13 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.AerobridgeTo;
				parameters13.AddWithValue("@aerobridgeto", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.Gpu90From.HasValue)
			{
				updateCmd += ",Gpu90From=@gpu90from";
				SqlParameterCollection parameters14 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.Gpu90From;
				parameters14.AddWithValue("@gpu90from", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.Gpu90To.HasValue)
			{
				updateCmd += ",Gpu90To=@gpu90to";
				SqlParameterCollection parameters15 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.Gpu90To;
				parameters15.AddWithValue("@gpu90to", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.Gpu180From.HasValue)
			{
				updateCmd += ",Gpu180From=@gpu180from";
				SqlParameterCollection parameters16 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.Gpu180From;
				parameters16.AddWithValue("@gpu180from", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.Gpu180To.HasValue)
			{
				updateCmd += ",Gpu180To=@gpu180to";
				SqlParameterCollection parameters17 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.Gpu180To;
				parameters17.AddWithValue("@gpu180to", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.Acu45From.HasValue)
			{
				updateCmd += ",Acu45From=@acu45from";
				SqlParameterCollection parameters18 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.Acu45From;
				parameters18.AddWithValue("@acu45from", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.Acu45To.HasValue)
			{
				updateCmd += ",Acu45To=@acu45to";
				SqlParameterCollection parameters19 = sqlCmd.Parameters;
				DateTime? schedule = flightInformation.Acu45To;
				parameters19.AddWithValue("@acu45to", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			}
			if (flightInformation.Qualifier != null)
			{
				updateCmd += ",Qualifier=@qualifier";
				sqlCmd.Parameters.AddWithValue("@qualifier", flightInformation.Qualifier ?? "");
			}
			if (flightInformation.Amsflightid != null)
			{
				updateCmd += ",Amsflightid=@amsflightid";
				sqlCmd.Parameters.AddWithValue("@amsflightid", flightInformation.Amsflightid ?? "");
			}
			if (flightInformation.Amslinkedflightid != null)
			{
				updateCmd += ",Amslinkedflightid=@amslinkedflightid";
				sqlCmd.Parameters.AddWithValue("@amslinkedflightid", flightInformation.Amslinkedflightid ?? "");
			}
			updateCmd += " WHERE id=@id";
			sqlCmd.Parameters.AddWithValue("@id", flightInformation.Id ?? "");
			sqlCmd.Parameters.AddWithValue("@actionname", flightInformation.ActionName ?? "");
			sqlCmd.Parameters.AddWithValue("@adi", flightInformation.Adi ?? "");
			sqlCmd.Parameters.AddWithValue("@linecode", flightInformation.LineCode ?? "");
			sqlCmd.Parameters.AddWithValue("@number", flightInformation.Number ?? "");
			sqlCmd.Parameters.AddWithValue("@scheduleddate", flightInformation.ScheduledDate ?? "");
			sqlCmd.CommandText = updateCmd;
			sqlCmd.CommandType = CommandType.Text;
			int i = sqlCmd.ExecuteNonQuery();
		}

		public static void InsertFlightInformation(SqlConnection connection, AHT_FlightInformation flightInformation)
		{
			SqlCommand sqlCmd = new SqlCommand("usp_Insert_AHT_FlightInformation", connection);
			sqlCmd.Parameters.AddWithValue("@Id", flightInformation.Id ?? "");
			sqlCmd.Parameters.AddWithValue("@ActionName", flightInformation.ActionName ?? "");
			sqlCmd.Parameters.AddWithValue("@Adi", flightInformation.Adi ?? "");
			sqlCmd.Parameters.AddWithValue("@LineCode", flightInformation.LineCode ?? "");
			sqlCmd.Parameters.AddWithValue("@Number", flightInformation.Number ?? "");
			sqlCmd.Parameters.AddWithValue("@ScheduledDate", flightInformation.ScheduledDate ?? "");
			sqlCmd.Parameters.AddWithValue("@Domesticintcode", flightInformation.Domesticintcode ?? "");
			sqlCmd.Parameters.AddWithValue("@City", flightInformation.City ?? "");
			sqlCmd.Parameters.AddWithValue("@Cityname", flightInformation.CityName ?? "");
			sqlCmd.Parameters.AddWithValue("@Aircraft", flightInformation.Aircraft ?? "");
			sqlCmd.Parameters.AddWithValue("@Paxcount", flightInformation.PaxCount ?? "");
			SqlParameterCollection parameters = sqlCmd.Parameters;
			DateTime? schedule = flightInformation.Schedule;
			parameters.AddWithValue("@Schedule", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters2 = sqlCmd.Parameters;
			schedule = flightInformation.Estimated;
			parameters2.AddWithValue("@Estimated", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters3 = sqlCmd.Parameters;
			schedule = flightInformation.Actual;
			parameters3.AddWithValue("@Actual", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			sqlCmd.Parameters.AddWithValue("@Gate", flightInformation.Gate ?? "");
			sqlCmd.Parameters.AddWithValue("@Status", flightInformation.Status ?? "");
			sqlCmd.Parameters.AddWithValue("@Remark", flightInformation.Remark ?? "");
			sqlCmd.Parameters.AddWithValue("@Rowfrom", flightInformation.RowFrom ?? "");
			sqlCmd.Parameters.AddWithValue("@Rowto", flightInformation.RowTo ?? "");
			sqlCmd.Parameters.AddWithValue("@Belt", flightInformation.Belt ?? "");
			SqlParameterCollection parameters4 = sqlCmd.Parameters;
			schedule = flightInformation.GateStart;
			parameters4.AddWithValue("@GateStart", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters5 = sqlCmd.Parameters;
			schedule = flightInformation.GateEnd;
			parameters5.AddWithValue("@GateEnd", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters6 = sqlCmd.Parameters;
			schedule = flightInformation.CounterStart;
			parameters6.AddWithValue("@CounterStart", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters7 = sqlCmd.Parameters;
			schedule = flightInformation.CounterEnd;
			parameters7.AddWithValue("@CounterEnd", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters8 = sqlCmd.Parameters;
			schedule = flightInformation.ClaimStart;
			parameters8.AddWithValue("@ClaimStart", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters9 = sqlCmd.Parameters;
			schedule = flightInformation.ClaimEnd;
			parameters9.AddWithValue("@ClaimEnd", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters10 = sqlCmd.Parameters;
			schedule = flightInformation.Mcdt;
			parameters10.AddWithValue("@Mcdt", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters11 = sqlCmd.Parameters;
			schedule = flightInformation.Mcat;
			parameters11.AddWithValue("@Mcat", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			sqlCmd.Parameters.AddWithValue("@Nopa", flightInformation.Nopa ?? "");
			sqlCmd.Parameters.AddWithValue("@Nopc", flightInformation.Nopc ?? "");
			sqlCmd.Parameters.AddWithValue("@Totalpax", flightInformation.Totalpax ?? "");
			sqlCmd.Parameters.AddWithValue("@Transit24", flightInformation.Transit24 ?? "");
			sqlCmd.Parameters.AddWithValue("@Potablewater", flightInformation.Potablewater ?? "");
			SqlParameterCollection parameters12 = sqlCmd.Parameters;
			schedule = flightInformation.AerobridgeFrom;
			parameters12.AddWithValue("@AerobridgeFrom", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters13 = sqlCmd.Parameters;
			schedule = flightInformation.AerobridgeTo;
			parameters13.AddWithValue("@AerobridgeTo", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters14 = sqlCmd.Parameters;
			schedule = flightInformation.Gpu90From;
			parameters14.AddWithValue("@Gpu90From", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters15 = sqlCmd.Parameters;
			schedule = flightInformation.Gpu90To;
			parameters15.AddWithValue("@Gpu90To", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters16 = sqlCmd.Parameters;
			schedule = flightInformation.Gpu180From;
			parameters16.AddWithValue("@Gpu180From", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters17 = sqlCmd.Parameters;
			schedule = flightInformation.Gpu180To;
			parameters17.AddWithValue("@Gpu180To", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters18 = sqlCmd.Parameters;
			schedule = flightInformation.Acu45From;
			parameters18.AddWithValue("@Acu45From", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			SqlParameterCollection parameters19 = sqlCmd.Parameters;
			schedule = flightInformation.Acu45To;
			parameters19.AddWithValue("@Acu45To", schedule.HasValue ? ((object)schedule.GetValueOrDefault()) : DBNull.Value);
			sqlCmd.Parameters.AddWithValue("@Qualifier", flightInformation.Qualifier ?? "");
			sqlCmd.Parameters.AddWithValue("@Amsflightid", flightInformation.Amsflightid ?? "");
			sqlCmd.Parameters.AddWithValue("@Amslinkedflightid", flightInformation.Amslinkedflightid ?? "");
			sqlCmd.CommandType = CommandType.StoredProcedure;
			int i = sqlCmd.ExecuteNonQuery();
		}

		public static void InsertCodeshareFlight(SqlConnection connection, AHT_FlightInformation flightInformation, string linecode, string number)
		{
			SqlCommand sqlCmd = new SqlCommand("usp_Insert_AHT_Codeshares", connection);
			sqlCmd.Parameters.AddWithValue("@IdFlightInformation", flightInformation.Id ?? "");
			sqlCmd.Parameters.AddWithValue("@LineCode", linecode ?? "");
			sqlCmd.Parameters.AddWithValue("@Number", number ?? "");
			sqlCmd.Parameters.AddWithValue("@ScheduledDate", flightInformation.ScheduledDate ?? "");
			sqlCmd.Parameters.AddWithValue("@Adi", flightInformation.Adi ?? "");
			sqlCmd.CommandType = CommandType.StoredProcedure;
			int i = sqlCmd.ExecuteNonQuery();
		}
	}

}
