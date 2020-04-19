How to execute the project in your local machine
Requirements
-Visual Studio 2019 (Cummunity edition is good)
-Sql server 2014 higher (Express edition is good)

1. Run the trading_engine_db_scripts.sql to the target database (ie.TradingEngine) need to update the database target in script if it is different.
2. Modify the db connection string under the TradingEngine.Api project (appsettings.json). Look for ConnectionStrings section
3. Set the TradingEngine.Api as the start up project and run
3. Start up page is a swagger documentation that will let you test the api endpoints
