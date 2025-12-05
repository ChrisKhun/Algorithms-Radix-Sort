import pandas as pd
import matplotlib.pyplot as plt

# Load CSV
df = pd.read_csv(
    "sort_results.csv",
    header=None,
    names=["timestamp", "algorithm", "dataset", "size", "time_ms", "mem_mb", "bytes_used"]
)

# Make sure numeric columns are numeric
df["time_ms"] = pd.to_numeric(df["time_ms"], errors="coerce")
df["mem_mb"] = pd.to_numeric(df["mem_mb"], errors="coerce")

datasets = df["dataset"].unique()

for d in datasets:
    subset = df[df["dataset"] == d]

    # Aggregate 10 runs: mean, min, max runtime per algorithm
    stats = subset.groupby("algorithm")["time_ms"].agg(["mean", "min", "max"]).sort_values("mean")

    # X positions
    x = range(len(stats))

    # Error bars: distance from mean to min/max
    yerr = [
        stats["mean"] - stats["min"],  # lower error
        stats["max"] - stats["mean"]   # upper error
    ]

    plt.figure(figsize=(8, 5))
    plt.bar(x, stats["mean"], yerr=yerr, capsize=5)
    plt.xticks(x, stats.index, rotation=45)
    plt.title(f"Runtime Comparison over 50 Runs\nDataset: {d}")
    plt.xlabel("Algorithm")
    plt.ylabel("Time (ms)")
    plt.grid(axis="y", linestyle="--", alpha=0.5)
    plt.tight_layout()
    plt.show()
