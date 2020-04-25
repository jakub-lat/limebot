import Vue from 'vue';
import JSONbig from 'json-bigint';

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
        r = await r.text();
        r = JSONbig.parse(r);
        return r;
    },
    async put(path, body) {
        var opts = defaultOpts();
        opts.method = 'put';
        opts.body = JSONbig.stringify(body);
        var r = await fetch(`${api}${path}`, opts);
        r = await r.text();
        r = JSONbig.stringify(r);
        return r;
    },
    path(path) {
        return `${api}${path}`;
    }
}