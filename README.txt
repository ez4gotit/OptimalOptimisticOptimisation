# README.txt

## Testing Instructions

### Organizing Tests

1. Place your test input data and expected output data in the corresponding directory.
   - Create two .txt files for each test case:
     - `test_n.txt` for input data.
     - `test_n_out.txt` for expected output data.

     WHERE n >= 1

### Configuration

2. Open `config.py` to configure paths:
   - Specify the path to the executable file that you want to test.
   - Define the path to the folder where your test files are located.

### Running Tests

3. To run the tests, you have two options:
   - Use the following command in your terminal:
     ```
     python3 test.py
     ```
   - Alternatively, run the provided `run_tests.sh` script.

4. The test results will be reported in the `tests
