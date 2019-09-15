import flask
from flask import request, jsonify
import pyodbc
import json
import requests

app = flask.Flask(__name__)
app.config["DEBUG"] = True

#DB Information
server = '<SQL Server Name>'
database = '<Database Name>'
username = '<DB User>'
password = '<DB Password>'

#Main page
@app.route('/',methods=['GET'])
def home():
    return "<h1>Internet - Api App</h1><p>The app is running.<p>"

#Query database directly
@app.route('/api/time',methods=['GET'])
def currentTime():
    sql_query = 'SELECT DATEDIFF_BIG(millisecond, \'1970-01-01 00:00:00\', GETUTCDATE()) AS currentDateTime'
    cnxn = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};SERVER='+server+';DATABASE='+database+';UID='+username+';PWD='+ password)
    cursor = cnxn.cursor()
    result = cursor.execute(sql_query)
    current = 0
    apiResponse = {}

    for row in result:
        current = row.currentDateTime
    
    apiResponse['UnixTimestamp'] = current
    return(jsonify(apiResponse))

#Query intranet database via intergration tier
@app.route('/api/intranet/time',methods=['GET'])
def intranetTime():
    r = requests.get('http://12.0.0.116/api/time?api_key=<API Umbrella Key>')
    data = json.loads(r.text)
    result = data['UnixTimestamp']
    apiResponse = {}

    apiResponse['UnixTimestamp'] = result
    return(jsonify(apiResponse))

@app.errorhandler(404)
def page_not_found(e):
    return "<h1>404</h1><p>The resource could not be found.</p>", 404

if __name__ == "__main__":
    app.run(host='0.0.0.0', port=5000)