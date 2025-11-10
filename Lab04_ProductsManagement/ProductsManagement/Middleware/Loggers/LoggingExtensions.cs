using Microsoft.Extensions.Logging;

namespace ProductsManagement.Features.Products;

public static partial class LoggingExtensions
{
    [LoggerMessage(
        EventId = 2003, 
        Level = LogLevel.Information, 
        Message = "[Op: {OperationId}] Metrics for {ProductName} ({SKU}, {Category}). Success: {Success}. Validation: {ValidationMs:N0}ms, DB: {DatabaseMs:N0}ms, Total: {TotalMs:N0}ms. Error: {ErrorReason}")]
    private static partial void LogProductCreationMetricsInternal(
        this ILogger logger,
        string OperationId,
        string ProductName,
        string SKU,
        ProductCategory Category,
        bool Success,
        string? ErrorReason,
        double ValidationMs,
        double DatabaseMs,
        double TotalMs);
    
    public static void LogProductCreationMetrics(this ILogger logger, ProductCreationMetrics metrics)
    {
        LogProductCreationMetricsInternal(
            logger,
            metrics.OperationId,
            metrics.ProductName,
            metrics.SKU,
            metrics.Category,
            metrics.Success,
            metrics.ErrorReason,
            metrics.ValidationDuration.TotalMilliseconds,
            metrics.DatabaseSaveDuration.TotalMilliseconds,
            metrics.TotalDuration.TotalMilliseconds);
    }

    [LoggerMessage(EventId = 2001, Level = LogLevel.Information, Message = "[Op: {OperationId}] Product creation started for {Name} (Brand: {Brand}, Category: {Category}, SKU: {SKU}).")]
    public static partial void LogProductCreationStarted(this ILogger logger, string OperationId, string Name, string Brand, ProductCategory Category, string SKU);
    
    [LoggerMessage(EventId = 2002, Level = LogLevel.Warning, Message = "[Op: {OperationId}] Product validation failed for {Name} (SKU: {SKU}). Reason: {ErrorReason}")]
    public static partial void LogProductValidationFailed(this ILogger logger, string OperationId, string Name, string SKU, string ErrorReason);
    
    [LoggerMessage(EventId = 2004, Level = LogLevel.Debug, Message = "[Op: {OperationId}] Database save operation started.")]
    public static partial void LogDatabaseOperationStarted(this ILogger logger, string OperationId);
    
    [LoggerMessage(EventId = 2005, Level = LogLevel.Debug, Message = "[Op: {OperationId}] Database save operation completed for ProductId: {ProductId}.")]
    public static partial void LogDatabaseOperationCompleted(this ILogger logger, string OperationId, Guid ProductId);

    [LoggerMessage(EventId = 2006, Level = LogLevel.Debug, Message = "[Op: {OperationId}] Cache operation (invalidation) performed for key: {CacheKey}.")]
    public static partial void LogCacheOperationPerformed(this ILogger logger, string OperationId, string CacheKey);
    
    [LoggerMessage(EventId = 2007, Level = LogLevel.Debug, Message = "[Op: {OperationId}] SKU uniqueness validation performed for SKU: {SKU}.")]
    public static partial void LogSKUValidationPerformed(this ILogger logger, string OperationId, string SKU);

    [LoggerMessage(EventId = 2008, Level = LogLevel.Debug, Message = "[Op: {OperationId}] Stock quantity validation performed for Product: {Name}.")]
    public static partial void LogStockValidationPerformed(this ILogger logger, string OperationId, string Name);
}