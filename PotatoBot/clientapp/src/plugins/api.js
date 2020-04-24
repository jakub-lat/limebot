import Vue from 'vue';

const api = '/api/';
const defaultOpts = () => ({
    headers: {
        Authorization: Vue.$cookies.get('token'),
        'Content-Type': 'application/json'
    },
});

export default {
    async get(path) {
        var r = await fetch(`${api}${path}`, defaultOpts());
        r = await r.json();
        return r;
    },
    async put(path, body) {
        var opts = defaultOpts();
        opts.method = 'put';
        opts.body = JSON.stringify(body);
        var r = await fetch(`${api}${path}`, opts);
        r = await r.json();
        return r;
    },
    path(path) {
        return `${api}${path}`;
    }
}