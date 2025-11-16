import pandas as pd
import matplotlib.pyplot as plt

# Load CSV
df = pd.read_csv("sort_results.csv", header=None,
                 names=["timestamp","algorithm","dataset","size","time_ms","mem_mb","bytes_used"])

# Example: plot runtimes by algorithm for each dataset
datasets = df.dataset.unique()

for d in datasets:
    subset = df[df["dataset"] == d]
    plt.figure()
    plt.plot(subset["algorithm"], subset["time_ms"])
    plt.title(f"Runtime Comparison ({d})")
    plt.xlabel("Algorithm")
    plt.ylabel("Time (ms)")
    plt.grid(True)
    plt.tight_layout()
    plt.show()
