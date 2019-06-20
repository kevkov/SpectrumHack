import React, {useState} from 'react';
import {Body, Button, Container, Footer, FooterTab, Header, Icon, Left, Right, Title, Text, Content, View} from "native-base";
import { Map } from '../../components/map';
import Constants from "expo-constants";
import { JourneyDetails } from './journeyDetails';

enum Tab  { MAP, DETAILS}

const getContent = (tab:Tab, props) => {
    if (tab == Tab.MAP) {
        return (<Map {...props} />);
    }
    return (<JourneyDetails />);
};
export const Route = (props) => {

    const [currentTab, setCurrentTab] = useState<Tab>(Tab.MAP);
    return (
        <Container style={{flex:1}}>
            <Header style={{paddingTop: Constants.statusBarHeight}}>
                <Left>
                    <Button
                        transparent
                        onPress={() => props.navigation.openDrawer()}>
                        <Icon name="menu"/>
                    </Button>
                </Left>
                <Body>
                    <Title>Routes</Title>
                </Body>
                <Right/>
            </Header>
            { getContent(currentTab, props) }
            <Footer>
                <FooterTab>
                    <Button
                        active={currentTab == Tab.MAP}
                        vertical
                        onPress={() => setCurrentTab(Tab.MAP)}
                    >
                        <Icon name="map" />
                        <Text>Map</Text>
                    </Button>
                    <Button
                        active={currentTab == Tab.DETAILS}
                        vertical
                        onPress={() => setCurrentTab(Tab.DETAILS)}>
                        <Icon name="list" />
                        <Text>Details</Text>
                    </Button>
                </FooterTab>
            </Footer>
        </Container>
    )
};
