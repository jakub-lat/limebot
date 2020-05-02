<template>
    <!--v-container>
        <h1 class="display-2 font-weight-bold mb-3 mt-5" id="start">
            Commands
        </h1>
        <v-col xl="9">
            <v-expansion-panels v-if="loaded" v-model="panels">
                <v-expansion-panel
                v-for="category in Object.keys(commands)"
                :key="category"
                >
                <v-expansion-panel-header class="text-capitalize">{{category}}</v-expansion-panel-header>
                <v-expansion-panel-content>
                    <v-list>
                        <v-subheader class="pa-0">
                            <v-col cols="6" md="4">COMMAND</v-col>
                            <v-col>DESCRIPTION</v-col>
                            <v-col cols="2" class="d-none d-md-block">ALIASES</v-col>
                        </v-subheader>
                        <v-list-item
                            v-for="(command, i) in commands[category]"
                            :key="i"
                            @click="open(command)"
                            class="pa-0"
                        >
                            <v-list-item-content>
                                <v-col cols="6" md="4">
                                    <pre class="text-wrap">{{getUsage(command)}}</pre>
                                </v-col>
                                <v-col>
                                    {{command.description}}
                                </v-col>
                                <v-col cols="2" class="d-none d-md-block">
                                    <pre class="text-wrap">{{command.aliases.join(', ')}}</pre>
                                </v-col>
                            </v-list-item-content>
                        </v-list-item>
                    </v-list>
                </v-expansion-panel-content>
                </v-expansion-panel>
            </v-expansion-panels>
            <v-progress-linear v-else indeterminate/>
        </v-col>
        <v-row justify="center">
            <v-dialog v-model="dialog" max-width="400">
                <v-card>
                    <v-card-title class="headline text-capitalize">{{command.name}}</v-card-title>
                    <v-card-text>
                        <p>{{command.description}}</p>
                        <p v-if="command.aliases.length > 0"><b>Aliases:</b> {{command.aliases.join(', ')}}</p>
                        <p>
                            <b>Usage:</b>
                            <pre>{{getUsage(command)}}</pre>
                        </p>
                    </v-card-text>
                    <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn @click="dialog = false" text color="primary">Close</v-btn>
                    </v-card-actions>
                </v-card>
            </v-dialog>
        </v-row>
    </v-container-->
    <v-container>
        <h1 class="display-2 font-weight-bold mb-6 mt-5">
            Commands
        </h1>
        <v-row no-gutters>
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
                                    <v-expansion-panel-header class="text-capitalize">
                                        {{command.name}}
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
        },
        tab: null,
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
    }
}
</script>