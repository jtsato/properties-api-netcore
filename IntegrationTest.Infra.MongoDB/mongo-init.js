print('Start #################################################################');

db = new Mongo().getDB("duckhome-mongodb");

db.createUser({
    user: 'xunit',
    pwd: 'xunit',
    roles: [
        {
            role: 'readWrite',
            db: 'duckhome-mongodb',
        },
    ],
});

db.createCollection('properties', {capped: false});
db.createCollection('properties_sequences', {capped: false});

print('End #################################################################');
