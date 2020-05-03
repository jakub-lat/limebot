<template>
    <v-container>
        <h1 class="display-1">Manage {{server.name}}</h1>
        <div class="d-flex pa-3 flex-wrap">
            <v-hover
                v-slot:default="{ hover }"
                v-for="panel in panels" :key="panel.title"
            >
                <v-card 
                    class="ma-3 pa-2" 
                    width="300" 
                    link append :to="panel.url"
                    :elevation="hover ? 12 : 2" 
                >
                    <v-card-text>
                    <h2 class="mb-3">
                        {{panel.title}} <v-icon class="headline float-right">{{panel.icon}}</v-icon>
                    </h2>
                    <div>
                        {{panel.description}}
                    </div>
                    </v-card-text>
                </v-card>
            </v-hover>
        </div>
    </v-container>
</template>
<script>
import store from '@/store';
export default {
    data: ()=>({
        panels: [
            {title: 'General settings', description: 'Configure general options', url: 'general', icon: 'mdi-cogs'},
            {title: 'Join / leave actions', description: 'Autoroles, welcome messages', url: 'join-leave', icon: 'mdi-account-plus'},
            {title: 'Log settings', description: 'Mod logs / message logs', url: 'logs', icon: 'mdi-text-box-multiple'},
            {title: 'Reaction roles', description: 'Reaction role settings', url: 'reaction-roles', icon: 'mdi-emoticon-wink'}
        ],
        server: store.state.server
    })
}
</script>