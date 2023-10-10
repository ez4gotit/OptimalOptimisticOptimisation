import subprocess
import os
from config import exe_path, tests_path


def run_test(input_file, output_file, exe_path):
    try:
        with open(input_file, "r") as input_txt, open(output_file, "r") as expected_txt:
            input_data = input_txt.read()
            expected_output = expected_txt.read()

        process = subprocess.Popen(
            [exe_path],
            stdin=subprocess.PIPE,
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
            text=True,
        )
        stdout, stderr = process.communicate(input=input_data)

        with open("tests_report.txt", "a") as out_file:
            out_file.write(f"\n{input_file}\n")
            out_file.write("Output: ")
            if stdout == expected_output:
                out_file.write("CORRECT\n")
            else:
                msg = "ERROR\nGot:\n" + stdout + "Expected:\n" + expected_output
                out_file.write(msg)

            if stderr:
                out_file.write("Errors:\n")
                out_file.write(stderr)

    except FileNotFoundError:
        with open("tests_report.txt", "a") as out_file:
            out_file.write(
                f"{input_file}: File not found. Please check the file paths.\n"
            )
    except Exception as e:
        with open("tests_report.txt", "a") as out_file:
            out_file.write(f"An error occurred while testing {input_file}: {str(e)}")


def main():
    test_files = [
        f
        for f in os.listdir(tests_path)
        if os.path.isfile(os.path.join(tests_path, f)) and f.startswith("test_")
    ]

    for test_file in test_files:
        run_test(
            os.path.join(tests_path, test_file),
            os.path.join(tests_path, test_file.replace(".txt", "_out.txt")),
            exe_path,
        )


if __name__ == "__main__":
    main()
