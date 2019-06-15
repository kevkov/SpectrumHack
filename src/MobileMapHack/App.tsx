import React from 'react';
import {StyleSheet} from "react-native";
import { Map } from './components/Map'
import { createDrawerNavigator, createStackNavigator, createAppContainer } from "react-navigation";
import { SideBar } from "./components/sidebar";
import { Home } from './screens/home'
import {Root} from "native-base";


const Drawer = createDrawerNavigator(
    {
        Home: { screen: Home }
    },
    {
        initialRouteName: "Home",
        contentOptions: {
            activeTintColor: "#e91e63"
        },
        contentComponent: props => <SideBar {...props} />
    }
);

const AppNavigator = createStackNavigator(
    {
        Drawer: { screen: Drawer }
    },
    {
        initialRouteName: "Drawer",
        headerMode: "none"
    }
);

const AppContainer = createAppContainer(AppNavigator);

export default function App() {
    return (
        <Root>
            <AppContainer />
        </Root>
    )
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#fff',
        alignItems: 'center',
        justifyContent: 'center',
    },
});
