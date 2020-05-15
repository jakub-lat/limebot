<template>
    <v-container>
        <h1 class="display-2 font-weight-bold mb-6 mt-5">
            Commands
        </h1>
        <v-row no-gutters class="mb-12 pb-12">
            <v-col xl="7">
            <v-card v-if="loaded" dark class="grey darken-4">
                <v-tabs
                v-model="tab"
                dark background-color="transparent"
                show-arrows center-active
                >
                    <v-tab
                        v-for="category in Object.keys(commands)"
                        :key="category"
                    >
                        {{ category }}
                    </v-tab>
                </v-tabs>
            
                <v-tabs-items v-model="tab">
                    <v-tab-item
                        v-for="(category) in Object.keys(commands)"
                        :key="category" class="grey darken-4"
                    >
                        <v-container class="pa-2 pa-md-4" >
                            <v-expansion-panels v-model="panels[category]">
                                <v-expansion-panel
                                    v-for="(command, i) in commands[category]"
                                    :key="i"
                                    class="blue-grey darken-4"
                                >
                                    <v-expansion-panel-header>
                                        <template v-slot:default="{ open }">
                                            <span style="max-width: 90%">
                                                <pre style="width: 60px" class="float-left">{{command.name}}</pre>
                                                <v-fade-transition>
                                                    <span class="text-truncate text--secondary ml-10 float-left" style="max-width: 50%" v-if="!open">
                                                        {{command.description}}
                                                    </span>
                                                </v-fade-transition>
                                                
                                            </span>
                                        </template>
                                    </v-expansion-panel-header>
                                    <v-expansion-panel-content >
                                        <p>{{command.description}}</p>
                                        <p v-if="command.aliases.length > 0">
                                            Aliases: <kbd class="d-inline-block">{{command.aliases.join(', ')}}</kbd>
                                        </p>
                                        <p>
                                            Usage:
                                            <pre v-for="(overload, i) in command.overloads" :key="i"><kbd>${{command.name}}{{getArguments(overload)}}</kbd></pre>
                                        </p>
                                    </v-expansion-panel-content>
                                </v-expansion-panel>
                            </v-expansion-panels>
                        </v-container>
                    </v-tab-item>
                </v-tabs-items>
            </v-card>
        <v-progress-linear v-else indeterminate/>
        </v-col>
        </v-row>
    </v-container>
</template>
<script>
export default {
    data: ()=>({
        commands: null,
        loaded: false,
        dialog: false,
        panels: {},
        command: {
            name: '',
            description: '',
            overloads: '',
            aliases: []
        }
    }),
    async created() {
        this.commands = await this.$api.get('commands');
        this.loaded = true;
    },
    methods: {
        getArguments(o) {
            var text = '';
            if(o.length == 0) return text;
            o.forEach(i=>{
                if(i.optional) {
                    text += ` [${i.name}]`;
                } else {
                    text += ` <${i.name}>`;
                }
            });
            
            return text;
        },
        open(cmd) {
            console.log(cmd);
            this.command = cmd;
            this.dialog = true;
        }
    },
    computed: {
        tab: {
            get() {
                let x = Object.keys(this.commands).findIndex(i=>i.toLowerCase() == this.$route.hash.substr(1).replace('-', ' '));
                return x == -1 ? 0 : x;
            },
            set(v) {
                location.hash = Object.keys(this.commands)[v].toLowerCase().replace(' ', '-');
            }
        }
    }
}
</script>