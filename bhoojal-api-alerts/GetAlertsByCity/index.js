const { CosmosClient } = require("@azure/cosmos");

const endpoint = process.env["CosmosEndpoint"];
const key = process.env["CosmosKey"];
const client = new CosmosClient({ endpoint, key });

module.exports = async function (context, req) {

    const { database } = await client.databases.createIfNotExists({ id: "bhoojal_outlets" });
    const  outletContainer  = await database.containers.createIfNotExists({ id: "outlet" });
    const  alertsContainer  = await database.containers.createIfNotExists({ id: "alerts" });

    const  outletData  = await outletContainer.container.items.query("SELECT * from c").fetchAll();
    const  alertData  = await alertsContainer.container.items.query("SELECT * from c").fetchAll();

    const outlets = outletData.resources.filter(({city}) => city == req.query.city);

    var alertsForCity = [];

    outlets.forEach(element => {
        var alert = alertData.resources.filter(({outletId}) => outletId == element.id);
        if(alert.length > 0){
            alertsForCity.push(alert);
        }
    });
    context.res = {
        // status: 200, /* Defaults to 200 */
        body: alertsForCity
    };
}   