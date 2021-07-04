const { v4: uuidv4 } = require('uuid');

module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');
    var geoJson = req.body;
    if (geoJson) {
        context.bindings.outputDocument = JSON.stringify({
            // create a random ID
            id: uuidv4(),
            scannedIn: geoJson.scannedIn,
            city: geoJson.city,
            rainFall: geoJson.rainFall,
            boundary: geoJson.boundary
        });
    }
    context.res = {
        // status: 200, /* Defaults to 200 */
        body: geoJson
    };
}

