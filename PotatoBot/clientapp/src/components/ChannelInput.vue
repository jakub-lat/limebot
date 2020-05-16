<template>
<v-autocomplete
    :label="label"
    :required="required"
    :disabled="disabled"
    width="200px"
    v-model="value"
    :items="items"
    outlined persistent-hint
/>
</template>
<script>
import store from '@/store';
export default {
    props: {
        id: String,
        label: String,
        required: Boolean,
        disabled: Boolean
    },
    computed: {
        value: {
            get() {
                return store.state.settings[this.id] || [];

            },
            set(val) {
                store.commit('set', {[this.id]: val});
            }
        },
        items() {
            var chn = store.state.server.channels.filter(i=>i.type=='Text').map(i=>({text:`#${i.name} (${i.parent || 'no category'})`, value: i.id}));
            return chn;
        }
    }
}
</script>