// MSMQDataWareHouse.Program
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Messaging;
using System.Text;
using System.Xml;
using MSMQDatabase;
using MSMQFlightDataFormat;

namespace MSMQDataWareHouse
{
	class Program
	{
		public static bool debugFlag = false;

		private static string flightLineCode = string.Empty;

		private static string flightNumber = string.Empty;

		private static string connectionString = "Data Source=SRV-DB;Initial Catalog=aht-site-corporation;Persist Security Info=True;User ID=sa;Password=AHT@2018";

		private static int localTimeZone = 7;

		private static void Main(string[] args)
		{
			try
			{
				if (ConfigurationManager.AppSettings["Debug"] != "0")
				{
					debugFlag = true;
				}
				connectionString = ConfigurationManager.ConnectionStrings["SQLCon"].ConnectionString;
				localTimeZone = Convert.ToInt32(ConfigurationManager.AppSettings["LocalTimezone"]);
				flightLineCode = ConfigurationManager.AppSettings["LineCode"];
				flightNumber = ConfigurationManager.AppSettings["Number"];
				MessageQueue messageQueue = new MessageQueue(ConfigurationManager.AppSettings["QueueName"]);
				string[] targetTypeNames = new string[1] { "System.String,mscorlib" };
				messageQueue.Formatter = new XmlMessageFormatter(targetTypeNames);
				messageQueue.PeekCompleted += MyPeekCompleted;
				messageQueue.ReceiveCompleted += MyReceiveCompleted;
				Console.WriteLine("Start listening message at....... " + DateTime.Now.ToString());
				messageQueue.BeginPeek();
				Console.ReadLine();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				ErrorLogging("./../log.txt", e.ToString());
			}
		}

		private static void MyPeekCompleted(object source, PeekCompletedEventArgs asyncResult)
		{
			Console.WriteLine("Detected a message on queue at... " + DateTime.Now.ToString());
			try
			{
				MessageQueue mq = (MessageQueue)source;
				Message i = mq.EndPeek(asyncResult.AsyncResult);
				mq.BeginReceive();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				ErrorLogging("./../log.txt", e.ToString());
			}
		}

		private static void MyReceiveCompleted(object source, ReceiveCompletedEventArgs asyncResult)
		{
			Console.WriteLine("Receive operation completed at... " + DateTime.Now.ToString());
			try
			{
				MessageQueue mq = (MessageQueue)source;
				Message i = mq.EndReceive(asyncResult.AsyncResult);
				ReadMessage(i);
				mq.BeginPeek();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				ErrorLogging("./../log.txt", e.ToString());
			}
			Console.WriteLine("------------------------------------------------------");
			Console.WriteLine("Start listening message at....... " + DateTime.Now.ToString());
		}

		private static void ReadMessage(Message message)
		{
			message.Formatter = new XmlMessageFormatter(new string[1] { "System.String,mscorlib" });
			StreamReader streamReader = new StreamReader(message.BodyStream, Encoding.Unicode);
			using (new StreamReader(message.BodyStream))
			{

				string xmlContent = streamReader.ReadToEnd().ToString();
				// Tao log  luu du lieu tho
				LogService logService= new LogService();
                if (logService.CheckLog())
                {
                    logService.UpdateLog(xmlContent);
                }
                else
                {
                    logService.AddLog(xmlContent);
                }

                int index = xmlContent.IndexOf("</connect>", StringComparison.Ordinal);
				xmlContent = xmlContent.Substring(0, index + 10);
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(xmlContent);
				AHT_FlightInformation AHTFlightInformation = ConvertXmlToFlightData(xmlDoc);
				if (AHTFlightInformation.Id == null)
				{
					return;
				}
				if (flightLineCode != string.Empty && flightNumber != string.Empty && AHTFlightInformation.LineCode == flightLineCode && AHTFlightInformation.Number == flightNumber)
				{
					ErrorLogging("./../" + flightLineCode + flightNumber + ".txt", xmlContent);
				}
				SqlConnection sqlConnection = FlightDatabase.GetDBConnection(connectionString);
				sqlConnection.Open();
				if (debugFlag)
				{
					Console.WriteLine("Open connection to database successful!");
				}
				try
				{
					FlightDatabase.InsertFlightInformation(sqlConnection, AHTFlightInformation);
					if (AHTFlightInformation.LineCode2 != null && AHTFlightInformation.Number2 != null)
					{
						FlightDatabase.InsertCodeshareFlight(sqlConnection, AHTFlightInformation, AHTFlightInformation.LineCode, AHTFlightInformation.Number);
						FlightDatabase.InsertCodeshareFlight(sqlConnection, AHTFlightInformation, AHTFlightInformation.LineCode2, AHTFlightInformation.Number2);
					}
					if (AHTFlightInformation.LineCode3 != null && AHTFlightInformation.Number3 != null)
					{
						FlightDatabase.InsertCodeshareFlight(sqlConnection, AHTFlightInformation, AHTFlightInformation.LineCode3, AHTFlightInformation.Number3);
					}
					if (AHTFlightInformation.LineCode4 != null && AHTFlightInformation.Number4 != null)
					{
						FlightDatabase.InsertCodeshareFlight(sqlConnection, AHTFlightInformation, AHTFlightInformation.LineCode4, AHTFlightInformation.Number4);
					}
					if (debugFlag)
					{
						Console.WriteLine("Save data to database successful!");
					}
				}
				catch (Exception e)
				{
					ErrorLogging("./../log.txt", e.ToString() + xmlContent);
				}
				finally
				{
					sqlConnection.Close();
					sqlConnection.Dispose();
				}
			}
		}

		private static AHT_FlightInformation ConvertXmlToFlightData(XmlDocument xml)
		{
			AHT_FlightInformation flightInformation = new AHT_FlightInformation();
			XmlNodeList dailyNodeList = xml.SelectNodes("/connect/daily");
			if (dailyNodeList.Count > 0)
			{
				foreach (XmlNode xn in dailyNodeList)
				{
					flightInformation.ActionName = xn.Attributes["action"].Value;
					flightInformation.Adi = xn.Attributes["adi"].Value;
					flightInformation.LineCode = xn.Attributes["linecode"].Value;
					flightInformation.Number = xn.Attributes["number"].Value;
					flightInformation.ScheduledDate = xn.Attributes["schedule_date"].Value;
					string[] idContent = new string[8] { "adi = ", flightInformation.Adi, " linecode = ", flightInformation.LineCode, " number = ", flightInformation.Number, " scheduled_date = ", flightInformation.ScheduledDate };
					flightInformation.Id = string.Concat(idContent);
					if (DateTime.TryParseExact(flightInformation.ScheduledDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var schValue))
					{
						flightInformation.ScheduledDate = schValue.ToUniversalTime().AddHours(localTimeZone).ToString("yyyy-MM-dd");
					}
				}
				XmlNodeList fieldNodeList = xml.SelectNodes("/connect/daily/field");
				if (fieldNodeList.Count > 0)
				{
					foreach (XmlNode xn2 in fieldNodeList)
					{
						switch (xn2.Attributes["name"].Value)
						{
							case "claim":
								flightInformation.Belt = xn2.Attributes["value"].Value;
								break;
							case "status":
								flightInformation.Status = xn2.Attributes["value"].Value;
								break;
							case "remark":
								flightInformation.Remark = xn2.Attributes["value"].Value;
								break;
							case "city":
								flightInformation.City = xn2.Attributes["value"].Value;
								break;
							case "domesticintcode":
								flightInformation.Domesticintcode = xn2.Attributes["value"].Value;
								break;
							case "gate":
								flightInformation.Gate = xn2.Attributes["value"].Value;
								break;
							case "aircraft":
								flightInformation.Aircraft = xn2.Attributes["value"].Value;
								break;
							case "most_confident_arrival_pax_count":
								flightInformation.PaxCount = xn2.Attributes["value"].Value;
								break;
							case "most_confident_departure_pax_count":
								flightInformation.PaxCount = xn2.Attributes["value"].Value;
								break;
							case "schedule":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var scheduleValue))
									{
										flightInformation.Schedule = scheduleValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "ata":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var ataValue))
									{
										flightInformation.Actual = ataValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "eta":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var etaValue))
									{
										flightInformation.Estimated = etaValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "etd":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var etdValue))
									{
										flightInformation.Estimated = etdValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "atd":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var atdValue))
									{
										flightInformation.Actual = atdValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "counter":
								if (!(xn2.Attributes["value"].Value != string.Empty))
								{
									break;
								}
								if (flightInformation.RowFrom != null && xn2.Attributes["value"].Value.Length < 3)
								{
									if (Convert.ToInt32(flightInformation.RowFrom) > Convert.ToInt32(xn2.Attributes["value"].Value))
									{
										flightInformation.RowFrom = xn2.Attributes["value"].Value;
									}
									else if (Convert.ToInt32(flightInformation.RowTo) < Convert.ToInt32(xn2.Attributes["value"].Value))
									{
										flightInformation.RowTo = xn2.Attributes["value"].Value;
									}
								}
								else
								{
									flightInformation.RowFrom = xn2.Attributes["value"].Value;
									flightInformation.RowTo = xn2.Attributes["value"].Value;
								}
								break;
							case "linecode":
								if (xn2.Attributes["value"].Value != string.Empty && xn2.Attributes["instance"].Value == "2")
								{
									flightInformation.LineCode2 = xn2.Attributes["value"].Value;
								}
								if (xn2.Attributes["value"].Value != string.Empty && xn2.Attributes["instance"].Value == "3")
								{
									flightInformation.LineCode3 = xn2.Attributes["value"].Value;
								}
								if (xn2.Attributes["value"].Value != string.Empty && xn2.Attributes["instance"].Value == "4")
								{
									flightInformation.LineCode4 = xn2.Attributes["value"].Value;
								}
								break;
							case "number":
								if (xn2.Attributes["value"].Value != string.Empty && xn2.Attributes["instance"].Value == "2")
								{
									flightInformation.Number2 = xn2.Attributes["value"].Value;
								}
								if (xn2.Attributes["value"].Value != string.Empty && xn2.Attributes["instance"].Value == "3")
								{
									flightInformation.Number3 = xn2.Attributes["value"].Value;
								}
								if (xn2.Attributes["value"].Value != string.Empty && xn2.Attributes["instance"].Value == "4")
								{
									flightInformation.Number4 = xn2.Attributes["value"].Value;
								}
								break;
							case "gatestart":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var gatestartValue))
									{
										flightInformation.GateStart = gatestartValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "gateend":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var gateendValue))
									{
										flightInformation.GateEnd = gateendValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "claimstart":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var claimstartValue))
									{
										flightInformation.ClaimStart = claimstartValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "claimend":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var claimendValue))
									{
										flightInformation.ClaimEnd = claimendValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "counterend":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var counterendValue))
									{
										flightInformation.CounterEnd = counterendValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "counterstart":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var counterstartValue))
									{
										flightInformation.CounterStart = counterstartValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "most_confident_departure_time":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var mcdtValue))
									{
										flightInformation.Mcdt = mcdtValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "most_confident_arrival_time":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var mcatValue))
									{
										flightInformation.Mcat = mcatValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "nopa":
								flightInformation.Nopa = xn2.Attributes["value"].Value;
								break;
							case "nopc":
								flightInformation.Nopc = xn2.Attributes["value"].Value;
								break;
							case "totalpax":
								flightInformation.Totalpax = xn2.Attributes["value"].Value;
								break;
							case "transit24":
								flightInformation.Transit24 = xn2.Attributes["value"].Value;
								break;
							case "potablewater":
								flightInformation.Potablewater = xn2.Attributes["value"].Value;
								break;
							case "aerobridgefrom":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var aerobridgefromValue))
									{
										flightInformation.AerobridgeFrom = aerobridgefromValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "aerobridgeto":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var aerobridgetoValue))
									{
										flightInformation.AerobridgeTo = aerobridgetoValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "gpu90from":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var gpu90fromValue))
									{
										flightInformation.Gpu90From = gpu90fromValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "gpu90to":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var gpu90toValue))
									{
										flightInformation.Gpu90To = gpu90toValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "gpu180from":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var gpu180fromValue))
									{
										flightInformation.Gpu180From = gpu180fromValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "gpu180to":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var gpu180toValue))
									{
										flightInformation.Gpu180To = gpu180toValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "acu45from":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var acu45fromValue))
									{
										flightInformation.Acu45From = acu45fromValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "acu45to":
								{
									if (DateTime.TryParseExact(xn2.Attributes["value"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var acu45toValue))
									{
										flightInformation.Acu45To = acu45toValue.ToUniversalTime().AddHours(localTimeZone);
									}
									break;
								}
							case "qualifier":
								flightInformation.Qualifier = xn2.Attributes["value"].Value;
								break;
							case "amsflightid":
								flightInformation.Amsflightid = xn2.Attributes["value"].Value;
								break;
							case "amslinkedflightid":
								flightInformation.Amslinkedflightid = xn2.Attributes["value"].Value;
								break;
						}
					}
				}
			}
			if (debugFlag)
			{
				Console.WriteLine(flightInformation.Id + "; " + flightInformation.Status);
			}
			return flightInformation;
		}

		public static void ErrorLogging(string filePath, string text)
		{
			if (!File.Exists(filePath))
			{
				File.Create(filePath).Dispose();
			}
			 StreamWriter writer = File.AppendText(filePath);
			writer.WriteLine("------------------------------------------------------");
			writer.WriteLine(DateTime.Now.ToString() + ": " + text);
		}
	}

}
