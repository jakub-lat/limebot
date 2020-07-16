<template>
    <not-found v-if="notFound" />
    <v-container v-else-if="loaded" class="pb-12">
        <h1 class="display-2 font-weight-bold mt-5 text-center">
            Leaderboard for {{server.guildName}}
        </h1>
        <h2 class="text-center font-weight-medium mb-6">Top 100 active users</h2>
        <v-card raised min-width="300" max-width="1000" class="mx-auto mb-12">
            <v-list>
                <div v-for="(user, i) in server.leaderboard" :key="i" class="my-2">
                    <v-list-item>
                        <div class="px-2">
                            <h2 class="ma-0 text--secondary font-weight-light">#{{i+1}}</h2>
                        </div>
                        <v-list-item-avatar tile size="50" class="mx-3">
                            <v-img :src="user.avatarURL" style="border-radius:50%"/>
                        </v-list-item-avatar>
                        <v-list-item-content>
                            <v-row>
                                <v-col class="d-flex justify-center align-center">
                                    <v-list-item-title>
                                        <span class="headline">{{user.username}}</span>
                                        <span class="text--secondary subtitle-1">#{{user.discriminator}}</span>
                                    </v-list-item-title>
                                </v-col>
                                <v-col cols="auto" class="mr-3 d-flex justify-center align-center">
                                    <p class="ma-0">
                                        <span class="title text--secondary font-weight-light mr-1">XP</span>
                                        <span class="title font-weight-bold">{{user.xp}}</span>
                                    </p>
                                </v-col>
                                <v-col cols="auto" class="d-flex justify-center align-center">
                                    <span class="title text--secondary font-weight-light mr-3">LVL</span>
                                    <v-progress-circular :value="user.nextLevelPercent" size="55" color="primary">
                                        <span class="title font-weight-bold">{{user.level}}</span>
                                    </v-progress-circular>
                                </v-col>
                            </v-row>
                            
                        </v-list-item-content>
                    </v-list-item>
                    <v-divider/>
                </div>
            </v-list>
        </v-card>
    </v-container>
    <v-container fluid v-else>
    <router-view v-if="loaded"/>
    <v-progress-linear v-else indeterminate color="primary"/>
    </v-container>
</template>
<script>
import NotFound from './NotFound';
export default {
    components: {NotFound},
    data: ()=>({
      loaded: false,
      notFound: false,
      server: null
    }),
    async created() {
        let id = this.$route.params.id;
        let server = await this.$api.get(`guild/${id}/leaderboard`);
        console.log(server);
        if(server != null) {
            this.server = server;
        } else {
            this.notFound = true;
        }
        this.loaded = true;

    }
}
</script>