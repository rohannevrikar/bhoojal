module.exports = async function (context, req) {
    var documents = context.bindings.documents;
    context.res = {
        body: documents
    };
}