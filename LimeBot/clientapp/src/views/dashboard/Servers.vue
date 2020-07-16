<template>
    <v-container>
        <h1 class="display-1 mb-3">Select server</h1>
        <h3>Logged in as {{this.user.username}}</h3>
        <div class="d-flex flex-wrap justify-center justify-sm-start pa-3">
            <router-link v-for="server in this.user.guilds" :key="server.id" :to="getUrl(server)" style="text-decoration: none !important">
                <v-tooltip bottom dark>
                    <template v-slot:activator="{on}">
                        <v-img 
                        :src="getImg(server)" 
                        :gradient="getGradient(server)"
                        v-on="on" 
                        width="70px" 
                        height="70px" 
                        class="ma-2 ma-sm-3 elevation-4" 
                        style="border-radius: 50%">
                            <v-row class="fill-height m-0 title" align="center" justify="center" v-if="!server.botOnGuild">
                                <v-icon>mdi-account-plus</v-icon>
                            </v-row>
                            <template v-slot:placeholder>
                                <v-row class="fill-height ma-0" align="center" justify="center">
                                <v-progress-circular indeterminate color="grey lighten-5"></v-progress-circular>
                                </v-row>
                            </template>
                        </v-img>
                    </template>
                    <span>{{server.name}}</span>
                </v-tooltip>
            </router-link>
        </div>
    </v-container>
</template>
<script>
import store from '@/store';
export default {
    methods: {
        getImg(server) {
            if(server.icon) {
                return `https://cdn.discordapp.com/icons/${server.id}/${server.icon}.png?size=128`;
            } else {
                return 'https://cdn.discordapp.com/embed/avatars/1.png';
            }
        },
        getGradient(server) {
            return server.botOnGuild ? null : '0deg, rgba(0,0,0,.75), rgba(0,0,0,.75)';
        },
        getUrl(server) {
            if(server.botOnGuild) {
                return `/manage/${server.id}`;
            } else {
                return `/invite?id=${server.id}`; // &guild_id
            }
        }
    },
    computed: {
        user: ()=>store.state.user
    }
}
</script>