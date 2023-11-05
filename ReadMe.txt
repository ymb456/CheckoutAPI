# CheckoutAPI

This project is implemented in ASP.NET Core and exposes an endpoint for checkout functionality.

## Endpoint

The project exposes the following endpoint:
- `POST http://localhost:5001/checkout`

### Expected Input

The expected input for the checkout endpoint is a JSON array of strings representing the IDs of the watches to be checked out.

### Expected Output

The expected output is a JSON object containing the total price of the checkout.

## Deliverables

- Compressed solution (ZIP file)
- README file (also available on GitHub)
- Commit history

## Running the Application

### Using Visual Studio
- Open the solution in Visual Studio.
- Press F5 to run the application. This should launch the Swagger UI in your default browser.
- If you run the solution from Visual Studio it will use random available port to run. If you want to run the solution on port 5001 please see "Running Standalone"

### Running Standalone
- Navigate to the project directory in the terminal.
- Run the following command to start the application:
- If you run the solution standalone it will run the solution on port 5001

CheckoutAPI.exe


The application will run on port 5001 by default. You can use the following `curl` command to test it:

curl.exe -X POST "https://localhost:5001/checkout" -H "Content-Type: application/json" -d "[\"001\", \"002\", \"001\", \"004\", \"003\"]"


#### Git Log history

commit b979b675c4f0e8ead6e4aa89abdecd1d9a338730 (HEAD -> master, origin/master)
Author: YEV\YevLocal <yev.berezovsky@gmail.com>
Date:   Sun Nov 5 15:14:13 2023 -0500

    Fixing minor bug : removed unnecessary check for number of items

commit 2d36405d993890be4998ed6ef212200cb1743065
Author: YEV\YevLocal <yev.berezovsky@gmail.com>
Date:   Sun Nov 5 14:51:06 2023 -0500

    Added more unit tests. Fixed broken unit tests

commit bf965715b18363561f361e7e3c0e782727f2d892
Author: YEV\YevLocal <yev.berezovsky@gmail.com>
Date:   Sun Nov 5 14:40:13 2023 -0500

    implementing unit tests

commit ca3dcd074d1342cdebf1727e9076ff496755bd4d
Author: YEV\YevLocal <yev.berezovsky@gmail.com>
Date:   Sun Nov 5 14:37:51 2023 -0500

    created unit tests

commit 85c5309c5886195090d8555c995cfcdf3fb90cac
Author: YEV\YevLocal <yev.berezovsky@gmail.com>
Date:   Sun Nov 5 14:33:29 2023 -0500

    Add implementation of CheckoutController, remove generated stub WeatherForecast