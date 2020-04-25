<template>
  <v-app>
    <v-navigation-drawer v-model="drawer" app clipped v-if="this.$route.meta.nav !== false && loaded">
      <v-list-item class="px-2 my-2">
        <v-list-item-avatar>
          <v-img :src="serverIcon"/>
        </v-list-item-avatar>
        <v-list-item-content>
          <v-list-item-title class="title">
            {{server.name}}
          </v-list-item-title>
          <v-list-item-subtitle>
            Manage server
          </v-list-item-subtitle>
        </v-list-item-content>
        
      </v-list-item>
      <v-divider/>
      <v-list dense>
        <v-list-item exact link :to="`/manage/${this.$route.params.id}/`">
          <v-list-item-action>
            <v-icon>mdi-view-dashboard</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Dashboard</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        <v-list-item exact link :to="`/manage/${this.$route.params.id}/general`">
          <v-list-item-action>
            <v-icon>mdi-cogs</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>General settings</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        <v-divider/>
        <v-list-item exact link to="/commands">
          <v-list-item-action>
            <v-icon>mdi-view-list</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Commands</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
      <template v-slot:append>
        <v-list-item exact link to="/manage" class="mb-3">
          <v-list-item-action>
              <v-icon>mdi-view-list</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>Switch server</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </template>
    </v-navigation-drawer>

    <v-app-bar app clipped-left color="primary">
      <v-app-bar-nav-icon @click.stop="drawer = !drawer" v-if="this.$route.meta.nav !== false" />
      <v-toolbar-title class="d-none d-sm-block">Lime Dashboard</v-toolbar-title>
      <v-spacer />
      <v-menu offset-y>
        <template v-slot:activator="{ on }">
          <v-btn icon :loading="!loaded" v-on="on" class="mr-1">
            <v-avatar>
              <v-img :src="`https://cdn.discordapp.com/avatars/${user.id}/${user.avatar}.png?size=128`"/>
            </v-avatar>
          </v-btn>
        </template>
        <v-list>
          <v-list-item>
            <v-list-item-title><b>{{user.username}}#{{user.discriminator}}</b></v-list-item-title>
          </v-list-item>
          <v-list-item to="/">
            <v-list-item-title>Back to website</v-list-item-title>
            <v-list-item-icon><v-icon>mdi-web</v-icon></v-list-item-icon>
          </v-list-item>
          <v-list-item to="/logout">
            <v-list-item-title>Logout</v-list-item-title>
            <v-list-item-icon><v-icon>mdi-login-variant</v-icon></v-list-item-icon>
          </v-list-item>
        </v-list>
      </v-menu>
    </v-app-bar>

    <v-content>
      <v-container fluid>
        <router-view v-if="loaded"/>
        <v-progress-linear v-else indeterminate color="primary"/>
      </v-container>
    </v-content>
    <v-snackbar v-model="unsaved" color="error" :timeout="timeout">
      You have unsaved changes!
      <v-btn color="primary" @click="save" :loading="saveLoading" :disabled="saveLoading">
        Save
      </v-btn>
    </v-snackbar>
  </v-app>
</template>

<script>
//import Vue from 'vue';
import store from '@/store';
export default {
  data () {
    return {
      drawer: null,
      timeout: 0,
      loaded: false,
      saveLoading: false
    };
  },
  methods: {
    async loadServer() {
      var id = this.$route.params.id;
      if(!id) {
        store.commit('unsaved', false);
        store.commit('setServer', {});
        store.commit('setSettings', {});
        return;
      }
      if(this.server.id == id) {
        return;
      }

      this.loaded = false;
      var server = await this.$api.get(`guild/${id}`);
      var info = await this.$api.get(`guild/${id}/info`);
      this.loaded = true;
      store.commit('setSettings', {...server, id});
      store.commit('setServer', {...this.user.guilds.find(i=>i.id==id), ...info});
    },
    async save() {
      try {
        this.saveLoading = true;
        var body = {...store.state.settings, id: this.$route.params.id};
        var r = await this.$api.put(`guild/${this.$route.params.id}`, body);
        if(r) {
          store.commit('unsaved', false);
          this.saveLoading = false;
        }
      } catch(err) {
        console.log(err);
      }
    }
  },
  watch: {
    '$route' : 'loadServer'
  },
  async created () {
    const token = this.$cookies.get('token');
    if(!token) window.location.href = this.$api.path('auth');
    else {
      try {
        var r = await this.$api.get('auth/user');
        if(!r.id) throw 'wrong user response';
        store.commit('setUser', r);
        this.loaded = true;
      } catch(e) {
        window.location.href = this.$api.path('auth');
      }
    }
    this.loadServer();
  },
  computed: {
    settings() {
      return store.state.settings;
    },
    unsaved() {
      return store.state.unsaved;
    },
    server() {
      return store.state.server;
    },
    user() {
      return store.state.user;
    },
    serverIcon() {
      var server = store.state.server;
      if(server.icon) {
          return `https://cdn.discordapp.com/icons/${server.id}/${server.icon}.png?size=128`;
      } else {
          return 'https://cdn.discordapp.com/embed/avatars/1.png';
      }
    }
  }
};
</script>
