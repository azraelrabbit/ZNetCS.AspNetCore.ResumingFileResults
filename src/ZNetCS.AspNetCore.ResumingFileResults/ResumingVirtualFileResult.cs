﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResumingVirtualFileResult.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The resuming virtual file result.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.ResumingFileResults
{
    #region Usings

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Net.Http.Headers;

    using ZNetCS.AspNetCore.ResumingFileResults.Infrastructure;

    #endregion

    /// <summary>
    /// The resuming virtual file result.
    /// </summary>
    public class ResumingVirtualFileResult : VirtualFileResult, IResumingFileResult
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ResumingVirtualFileResult"/> class.
        /// </summary>
        /// <param name="fileName">
        /// The path to the file. The path must be relative/virtual.
        /// </param>
        /// <param name="contentType">
        /// The Content-Type header of the response.
        /// </param>
        /// <param name="etag">
        /// The ETag header of the response.
        /// </param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "OK")]
        public ResumingVirtualFileResult(string fileName, string contentType, string etag = null)
            : this(fileName, MediaTypeHeaderValue.Parse(contentType), !string.IsNullOrEmpty(etag) ? EntityTagHeaderValue.Parse(etag) : null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResumingVirtualFileResult"/> class.
        /// </summary>
        /// <param name="fileName">
        /// The path to the file. The path must be relative/virtual.
        /// </param>
        /// <param name="contentType">
        /// The Content-Type header of the response.
        /// </param>
        /// <param name="etag">
        /// The ETag header of the response.
        /// </param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "OK")]
        public ResumingVirtualFileResult(string fileName, MediaTypeHeaderValue contentType, EntityTagHeaderValue etag = null) : base(fileName, contentType)
        {
            this.EntityTag = etag;
        }

        #endregion

        #region Public Properties

        /// <inheritdoc/>
        public string FileInlineName { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var executor = context.HttpContext.RequestServices.GetRequiredService<ResumingVirtualFileResultExecutor>();
            return executor.ExecuteAsync(context, this);
        }

        #endregion
    }
}