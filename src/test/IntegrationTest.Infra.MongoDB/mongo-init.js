print('Start #################################################################');

db = new Mongo().getDB("properties-mongodb");

db.createUser({
    user: 'xunit',
    pwd: 'xunit',
    roles: [
        {
            role: 'readWrite',
            db: 'properties-mongodb',
        },
    ],
});

db.createCollection('properties', {capped: false});
db.createCollection('properties_sequences', {capped: false});

print('End #################################################################');
