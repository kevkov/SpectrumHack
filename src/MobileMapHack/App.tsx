import React, {Component} from 'react';
import {StyleSheet} from "react-native";
import { Map } from './components/map'
import { createDrawerNavigator, createStackNavigator, createAppContainer } from "react-navigation";
import { SideBar } from "./components/sidebar";
import { Home } from './screens/home'
import {Root} from "native-base";
import * as Font from 'expo-font';
import { Ionicons } from '@expo/vector-icons';
// @ts-ignore
import Roboto from 'native-base/Fonts/Roboto.ttf';
// @ts-ignore
import Roboto_medium from 'native-base/Fonts/Roboto_medium.ttf';
// @ts-ignore
import MaterialIcons from "native-base/Fonts/MaterialIcons.ttf";

const Drawer = createDrawerNavigator(
    {
        Home: { screen: Home },
        Map: { screen: Map }
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

export default class App extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isReady: false
        };
    }
    componentWillMount() {
        this.loadFonts();
    }
    async loadFonts() {
        await Font.loadAsync({
            Roboto,
            Roboto_medium,
            MaterialIcons,
           ...Ionicons.font
        });
        this.setState({ isReady: true });
    }

    render() {
        return (
            <Root>
                <AppContainer/>
            </Root>
        )
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#fff',
        alignItems: 'center',
        justifyContent: 'center',
    },
});
