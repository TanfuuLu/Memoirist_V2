﻿namespace Memoirist_V2.YarpGateway.Models;

public class SmtpSetting {
	public string Server { get; set; }
	public int Port { get; set; }
	public string SenderName { get; set; }
	public string SenderEmail { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
}