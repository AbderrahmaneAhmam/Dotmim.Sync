﻿using Dotmim.Sync.Batch;
using Dotmim.Sync.Builders;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dotmim.Sync
{
    public interface IProvider 
    {

        /// <summary>
        /// Set the token for the current operation
        /// </summary>
        void SetCancellationToken(CancellationToken token);

        /// <summary>
        /// Begin Session
        /// </summary>
        Task<SyncContext> BeginSessionAsync(SyncContext context);

        /// <summary>
        /// Event Progress
        /// </summary>
        event EventHandler<SyncProgressEventArgs> SyncProgress;

        event EventHandler<ApplyChangeFailedEventArgs> ApplyChangedFailed;

        /// <summary>
        /// Ensure scopes are created on both local and remote 
        /// If clientReferenceId is specified, we are on the server side and we need the client reference scope (with server timestamp)
        /// </summary>
        /// <param name="scopeName"></param>
        /// <param name="clientReferenceId"></param>
        /// <returns></returns>
        Task<(SyncContext, List<ScopeInfo>)> EnsureScopesAsync(SyncContext context, String scopeName, Guid? clientReferenceId = null);

        /// <summary>
        /// Ensure Configuration is correct and tables are get from datastore
        /// </summary>
        Task<(SyncContext, SyncConfiguration)> EnsureConfigurationAsync(SyncContext context, SyncConfiguration configuration = null);

        /// <summary>
        /// Ensure database is ready and created
        /// </summary>
        Task<SyncContext> EnsureDatabaseAsync(SyncContext context, ScopeInfo scopeInfo);

        /// <summary>
        /// Apply changes to the local storage, coming from this scope
        /// </summary>
        Task<(SyncContext, ChangesStatistics)> ApplyChangesAsync(SyncContext context, ScopeInfo fromScope, BatchInfo changes);

        /// <summary>
        /// Get Changes to be applied 
        /// </summary>
        Task<(SyncContext, BatchInfo, ChangesStatistics)> GetChangeBatchAsync(SyncContext context, ScopeInfo scopeInfo);

        /// <summary>
        /// Update scope to reflect last changed timestamp
        /// </summary>
        Task<SyncContext> WriteScopesAsync(SyncContext context, List<ScopeInfo> scopes);

        /// <summary>
        /// End Session
        /// </summary>
        Task<SyncContext> EndSessionAsync(SyncContext context);

        /// <summary>
        /// Get a local timestamp
        /// </summary>
        Task<(SyncContext, Int64)> GetLocalTimestampAsync(SyncContext context);
    }
}
