<template>
    <v-container>
        <h1 class="display-1 mb-4">Ranking system</h1>
        <v-hover v-slot:default="{ hover }">
            <v-card 
                color="grey darken-4" :elevation="hover ? 6 : 3" max-width="400"
                link to="/commands/#ranking"
            >
                <v-list-item>
                    <v-list-item-content>
                        <v-list-item-title>Learn more about ranking</v-list-item-title>
                    </v-list-item-content>

                    <v-list-item-action>
                        <v-icon>mdi-arrow-right</v-icon>
                    </v-list-item-action>
                </v-list-item>
            </v-card>
        </v-hover>
        <v-hover v-slot:default="{ hover }">
            <v-card 
                color="grey darken-4" :elevation="hover ? 6 : 3" max-width="400" class="mt-3"
                link :to="`/leaderboard/${server.id}`"
            >
                <v-list-item>
                    <v-list-item-content>
                        <v-list-item-title>View leaderboard for this server</v-list-item-title>
                    </v-list-item-content>

                    <v-list-item-action>
                        <v-icon>mdi-arrow-right</v-icon>
                    </v-list-item-action>
                </v-list-item>
            </v-card>
        </v-hover>

        <v-row>
            <v-col md="6">
                <v-row class="mb-2">
                    <v-col>
                        <h2>Enable XP/Leveling system</h2>
                    </v-col>
                    <v-col cols="auto">
                        <switch-input id="enableLeveling" class="ma-0" v-model="levelingEnabled"/>
                    </v-col>
                </v-row>
                

                <range-slider-input 
                    minId="minMessageXP"
                    maxId="maxMessageXP"
                    label="XP every minute"
                    :disabled="!levelingEnabled"
                    :min="2"
                    :max="100"
                />
                <slider-input 
                    id="requiredXPToLevelUp"
                    label="XP to level up"
                    :disabled="!levelingEnabled"
                    :min="5"
                    :max="2000"
                />

                <v-row class="mt-10">
                    <v-col>
                        <h2 :class="levelingEnabled ? '' : 'text--secondary'">Level up message</h2>
                    </v-col>
                    <v-col cols="auto">
                        <switch-input id="enableLevelUpMessage" class="ma-0" v-model="levelUpMessageEnabled" :disabled="!levelingEnabled"/>
                    </v-col>
                </v-row>

                <channel-input
                    id="levelUpMessageChannel"
                    label="Channel"
                    :disabled="!levelUpMessageEnabled || !levelingEnabled"
                    class="mb-4" enableCurrentChannel/>

                <my-input 
                    id="levelUpMessage" 
                    label="Level up message" 
                    :disabled="!levelUpMessageEnabled || !levelingEnabled" 
                    textarea maxlength="500" 
                    hint="Insert: {user} - user mention, {level} - level that they advanced to"
                    class="mb-4"/>

                <h2 :class="'mt-10 ' + (levelingEnabled ? '' : 'text--secondary')">Roles for level</h2>
                <v-row v-for="(x, i) in settings.rolesForLevel || []" :key="i">
                    <v-col>
                        <v-row>
                            <v-col cols="12" md="4">
                                Level {{x.level}}
                            </v-col>
                            <v-col>
                                @{{(server.roles.find(r=>r.id.toString() == x.role) || {}).name}} ({{x.role}})
                            </v-col>
                        </v-row>
                    </v-col>
                    <v-col cols="auto">
                        <v-btn fab @click="removeLevelRole(i)" small><v-icon>mdi-delete</v-icon></v-btn>
                    </v-col>
                </v-row>
                <h3>Add new</h3>
                <v-row no-gutters class="mt-2">
                    <v-col cols="12" md="3" class="mr-1">
                        <v-autocomplete
                            label="Level"
                            :items="levels"
                            :disabled="!levelingEnabled"
                            outlined
                            v-model="level"
                            />
                    </v-col>
                    <v-col class="mx-1">
                        <role-input :disabled="!levelingEnabled" single label="Role" v-model="role"></role-input>
                    </v-col>
                    <v-col cols="12" md="auto" class="ml-1">
                        <v-btn fab @click="addLevelRole" :disabled="!levelingEnabled" color="primary"><v-icon>mdi-plus</v-icon></v-btn>
                    </v-col>
                </v-row>

                <v-row class="mt-10">
                    <v-col>
                        <h2>Enable reputation system</h2>
                    </v-col>
                    <v-col cols="auto">
                        <switch-input id="enableReputation" v-model="reputationEnabled" class="ma-0"/>
                    </v-col>
                </v-row>
                <p>Enable reputation system - the "thanks @member" message will give the target member additional XP.</p>
                <slider-input 
                    id="reputationXP"
                    label="XP to get"
                    :disabled="!reputationEnabled"
                    :min="2"
                    :max="100"
                />
            </v-col>
        </v-row>
    </v-container>
</template>
<script>
import SwitchInput from '@/components/SwitchInput.vue';
import ChannelInput from '@/components/ChannelInput.vue';
import RoleInput from '@/components/RoleInput.vue';
import MyInput from '@/components/MyInput.vue';
import SliderInput from '@/components/SliderInput.vue';
import RangeSliderInput from '@/components/RangeSliderInput.vue';

import store from '@/store';

export default {
    components: {SwitchInput, MyInput, SliderInput, RangeSliderInput, ChannelInput, RoleInput},
    data: ()=>({
        levelingEnabled: false,
        levelUpMessageEnabled: true,
        reputationEnabled: false,
        levels: Array(200).fill(1).map((_, i)=>i+1),
        level: null,
        role: null
    }),
    methods: {
        addLevelRole() {
            console.log(this.level, this.role);
            console.log(this.settings);
            if(this.level == null || this.role == null) return;
            let newobj = {level: this.level, role: this.role};
            store.commit('set', {rolesForLevel: [...this.settings.rolesForLevel, newobj]})
        },
        removeLevelRole(index) {
            store.commit('set', {rolesForLevel: this.settings.rolesForLevel.filter((x, i)=>i!=index)});
        }
    },
    computed: {
        server: () => store.state.server,
        settings: () => store.state.settings
    }
}
</script>