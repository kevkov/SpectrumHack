import React, {Component} from 'react';
import {StyleSheet} from "react-native";
import { createDrawerNavigator, createStackNavigator, createAppContainer } from "react-navigation";
import { SideBar } from "./components/sidebar";
import { Home } from './screens/home'
import { Route } from './screens/route'
import {Root} from "native-base";
import * as Font from 'expo-font';
import { Ionicons } from '@expo/vector-icons';
// @ts-ignore
import Roboto from 'native-base/Fonts/Roboto.ttf';
// @ts-ignore
import Roboto_medium from 'native-base/Fonts/Roboto_medium.ttf';
// @ts-ignore
import MaterialIcons from "native-base/Fonts/MaterialIcons.ttf";
import {AppLoading} from "expo";

const Drawer = createDrawerNavigator(
    {
        Home: { screen: Home },
        Route: { screen: Route }
    },
    {
        initialRouteName: "Route",
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

export default class App extends Component<{}, {isReady: boolean}> {
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
        if (!this.state.isReady) {
            return (
                <AppLoading
                    onFinish={() => this.setState({isReady: true})}
                    onError={console.warn}
                />
            );
        }

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
