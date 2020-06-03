<template>
<v-autocomplete
    :label="label"
    :required="required"
    :disabled="disabled"
    width="200px"
    v-model="value"
    :items="items"
    outlined
    :multiple="!single"
    :chips="!single" :small-chips="!single" :deletable-chips="!single"
    hint="Don't see a role? This is probably because bot is below it. Try to move the bot up." persistent-hint
/>
</template>
<script>
import store from '@/store';
export default {
    props: {
        id: String,
        label: String,
        required: Boolean,
        disabled: Boolean,
        single: Boolean
    },
    computed: {
        value: {
            get() {
                return store.state.settings[this.id] || [];

            },
            set(val) {
                store.commit('set', {[this.id]: val});
                this.$emit('input', val);
            }
        },
        items() {
            var roles = store.state.server.roles.map(i=>({text:i.name, value: i.id}));
            return roles;
        }
    }
}
</script>