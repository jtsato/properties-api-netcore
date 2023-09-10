print('Start #################################################################');

db = new Mongo().getDB("duckhome-mongodb");

db.createUser({
    user: 'compose',
    pwd: 'compose',
    roles: [
        {
            role: 'readWrite',
            db: 'duckhome-mongodb',
        },
    ],
});

db.createCollection('properties', {capped: false});

print('End #################################################################');
