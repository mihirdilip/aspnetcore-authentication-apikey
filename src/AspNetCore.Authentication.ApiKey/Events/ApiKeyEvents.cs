// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using System;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.ApiKey
{
    /// <summary>
    /// ApiKey Events.
    /// </summary>
    public class ApiKeyEvents 
    {
        /// <summary>
        /// A delegate assigned to this property will be invoked just before validating api key. 
        /// </summary>
        /// <remarks>
        /// You must provide a delegate for this property for authentication to occur.
        /// In your delegate you should either call context.ValidationSucceeded() which will handle construction of authentication principal which will be assiged the context.Principal property and call context.Success(),
        /// or construct an authentication principal &amp; attach it to the context.Principal property and finally call context.Success() method.
        /// If only context.Principal property set without calling context.Success() method then, Success() method is automaticalled called.
        /// </remarks>
        public Func<ApiKeyValidateKeyContext, Task> OnValidateKey { get; set; }

        /// <summary>
        /// A delegate assigned to this property will be invoked when the authentication succeeds. It will not be called if <see cref="OnValidateKey"/> delegate is assigned.
        /// It can be used for adding claims, headers, etc to the response.
        /// </summary>
        /// <remarks>
        /// Only use this if you know what you are doing.
        /// </remarks>
        public Func<ApiKeyAuthenticationSucceededContext, Task> OnAuthenticationSucceeded { get; set; }

        /// <summary>
        /// A delegate assigned to this property will be invoked when the authentication fails.
        /// </summary>
        public Func<ApiKeyAuthenticationFailedContext, Task> OnAuthenticationFailed { get; set; } 

        /// <summary>
        /// A delegate assigned to this property will be invoked before a challenge is sent back to the caller when handling unauthorized response.
        /// </summary>
        /// <remarks>
        /// Only use this if you know what you are doing and if you want to use custom implementation.
        /// Set the delegate to deal with 401 challenge concerns, if an authentication scheme in question
        /// deals an authentication interaction as part of it's request flow. (like adding a response header, or
        /// changing the 401 result to 302 of a login page or external sign-in location.)
        /// Call context.Handled() at the end so that any default logic for this challenge will be skipped.
        /// </remarks>
        public Func<ApiKeyHandleChallengeContext, Task> OnHandleChallenge { get; set; } 

        /// <summary>
        /// A delegate assigned to this property will be invoked if Authorization fails and results in a Forbidden response.
        /// </summary>
        /// <remarks>
        /// Only use this if you know what you are doing and if you want to use custom implementation.
        /// Set the delegate to handle Forbid.
        /// Call context.Handled() at the end so that any default logic will be skipped.
        /// </remarks>
        public Func<ApiKeyHandleForbiddenContext, Task> OnHandleForbidden { get; set; } 





        /// <summary>
        /// Invoked when validating api key.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>A Task.</returns>
        public virtual Task ValidateKeyAsync(ApiKeyValidateKeyContext context) => OnValidateKey == null ? Task.CompletedTask : OnValidateKey(context);

        /// <summary>
        /// Invoked when the authentication succeeds.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>A Task.</returns>
        public virtual Task AuthenticationSucceededAsync(ApiKeyAuthenticationSucceededContext context) => OnAuthenticationSucceeded == null ? Task.CompletedTask : OnAuthenticationSucceeded(context);

        /// <summary>
        /// Invoked when the authentication fails.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>A Task.</returns>
        public virtual Task AuthenticationFailedAsync(ApiKeyAuthenticationFailedContext context) => OnAuthenticationFailed == null ? Task.CompletedTask : OnAuthenticationFailed(context);

        /// <summary>
        /// Invoked before a challenge is sent back to the caller when handling unauthorized response.
        /// </summary>
        /// <remarks>
        /// Override this method to deal with 401 challenge concerns, if an authentication scheme in question
        /// deals an authentication interaction as part of it's request flow. (like adding a response header, or
        /// changing the 401 result to 302 of a login page or external sign-in location.)
        /// Call context.Handled() at the end so that any default logic for this challenge will be skipped.
        /// </remarks>
        /// <param name="context"></param>
        /// <returns>A Task.</returns>
        public virtual Task HandleChallengeAsync(ApiKeyHandleChallengeContext context) => OnHandleChallenge == null ? Task.CompletedTask : OnHandleChallenge(context);

        /// <summary>
        /// Invoked if Authorization fails and results in a Forbidden response.
        /// </summary>
        /// <remarks>
        /// Override this method to handle Forbid.
        /// Call context.Handled() at the end so that any default logic will be skipped.
        /// </remarks>
        /// <param name="context"></param>
        /// <returns>A Task.</returns>
        public virtual Task HandleForbiddenAsync(ApiKeyHandleForbiddenContext context) => OnHandleForbidden == null ? Task.CompletedTask : OnHandleForbidden(context);
    }
}
