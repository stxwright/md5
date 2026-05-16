import sys
import os

def update_readme(bench_file_path):
    if not os.path.exists(bench_file_path):
        print(f"Benchmark file {bench_file_path} not found.")
        return

    with open(bench_file_path, 'r') as f:
        bench_content = f.read()

    # Find the table in the benchmark content
    # BenchmarkDotNet github format usually starts with a summary table
    lines = bench_content.split('\n')
    table_lines = []
    in_table = False
    for line in lines:
        if line.startswith('|'):
            in_table = True
            table_lines.append(line)
        elif in_table:
            break

    if not table_lines:
        print("No benchmark table found in the report.")
        return

    table_text = '\n'.join(table_lines)

    readme_path = 'README.md'
    if not os.path.exists(readme_path):
        readme_content = "# MD5 Implementation\n\n## Benchmarks\n\n" + table_text
    else:
        with open(readme_path, 'r') as f:
            readme_content = f.read()

        start_marker = "<!-- BENCHMARK_START -->"
        end_marker = "<!-- BENCHMARK_END -->"

        if start_marker in readme_content and end_marker in readme_content:
            before = readme_content.split(start_marker)[0]
            after = readme_content.split(end_marker)[1]
            readme_content = f"{before}{start_marker}\n\n{table_text}\n\n{end_marker}{after}"
        else:
            readme_content += f"\n\n## Benchmarks\n\n{start_marker}\n\n{table_text}\n\n{end_marker}\n"

    with open(readme_path, 'w') as f:
        f.write(readme_content)
    print("README.md updated with benchmark results.")

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python update_readme_benchmarks.py <path_to_benchmark_report>")
    else:
        update_readme(sys.argv[1])
