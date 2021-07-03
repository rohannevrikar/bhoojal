function main(consumedQuota, quota, phLevel, hardness) {
    let isAlert = false;
    let alertMetrics = [];

    // Check if consumption quota is exceeded
    if (consumedQuota > quota) {
        isAlert = true;
        alertMetrics.push({ metric: "consumption", value: consumedQuota, threshold: quota });
    }

    // Check if hardness is at unhealthy levels
    if (hardness > 180) {
        isAlert = true;
        alertMetrics.push({ metric: "hardness", value: hardness, threshold: 180 });
    }

    // Check if Ph level is too basic
    if (phLevel < 6.5) {
        isAlert = true;
        alertMetrics.push({ metric: "phLevelLow", value: phLevel, threshold: 6.5 });
    }

    // Check if Ph level is too acidic
    if (phLevel > 8) {
        isAlert = true;
        alertMetrics.push({ metric: "phLevelHigh", value: phLevel, threshold: 8 });
    }

    return {
        isAlert: isAlert,
        metrics: alertMetrics
    };
}