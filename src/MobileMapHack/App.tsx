import React from 'react';
import {StyleSheet} from "react-native";
import {Map} from './components/Map'
export default function App() {
    return Map();
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#fff',
        alignItems: 'center',
        justifyContent: 'center',
    },
});
