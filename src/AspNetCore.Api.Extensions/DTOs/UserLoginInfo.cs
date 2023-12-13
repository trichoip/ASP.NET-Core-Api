﻿namespace AspNetCore.Api.Extensions.DTOs;

public class UserLoginInfo
{
    public UserLoginInfo(string loginProvider, string providerKey, string? displayName)
    {
        LoginProvider = loginProvider;
        ProviderKey = providerKey;
        ProviderDisplayName = displayName;
    }

    public string LoginProvider { get; set; }

    public string ProviderKey { get; set; }

    public string? ProviderDisplayName { get; set; }
}
