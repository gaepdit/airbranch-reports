﻿using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.AspNetCore;
using System.Security.Claims;

namespace WebApp.Platform.Raygun;

public class RaygunClientProvider : DefaultRaygunAspNetCoreClientProvider
{
    public override RaygunClient GetClient(RaygunSettings settings, HttpContext context)
    {
        var client = base.GetClient(settings, context);
        client.ApplicationVersion = typeof(Program).Assembly.GetName().Version?.ToString(3);

        client.SendingMessage += (_, args) =>
        {
            args.Message.Details.Tags = args.Message.Details.Tags is null ? [] : args.Message.Details.Tags.ToList();
            args.Message.Details.Tags.Add(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        };

        var identity = context.User.Identity as ClaimsIdentity;
        if (identity?.IsAuthenticated == true)
            client.UserInfo = new RaygunIdentifierMessage(identity.Name) { IsAnonymous = false };

        return client;
    }
}
