﻿namespace Memoirist_V2.YarpGateway.Models;

public class ResetPasswordRequest {


	public string Email { get; set; }
	public string Code { get; set; }
	public string NewPassword { get; set; }
}
public class VerifyCodeRequest {
	public string Email { get; set; }
	public string Code { get; set; }
}