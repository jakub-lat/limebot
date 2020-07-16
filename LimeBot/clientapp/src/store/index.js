import Vuex from 'vuex'
import Vue from 'vue'

Vue.use(Vuex)

var store = new Vuex.Store({
  state: {
    settings: {},
    server: {},
    unsaved: false,
    user: {},
    token: null
  },
  mutations: {
    set (state, obj) {
        state.settings = {...state.settings, ...obj};
        state.unsaved = true;
    },
    setSettings(state, obj) {
        state.settings = obj;
        state.unsaved = false;
    },
    setServer(state, obj) {
        state.server = obj;
    },
    unsaved(state, val) {
        state.unsaved = !!val;
    },
    setUser(state, obj) {
      state.user = obj;
    }
  }
});

export default store;