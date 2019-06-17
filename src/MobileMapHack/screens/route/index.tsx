import React from 'react';
import {Body, Button, Container, Footer, FooterTab, Header, Icon, Left, Right, Title, Text} from "native-base";
import { Map } from '../../components/map';
import Constants from "expo-constants";

export const Route = (props) => {
    return (
        <Container>
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
            <Map {...props} />
            <Footer>
                <FooterTab>
                    <Button vertical>
                        <Icon name="map" />
                        <Text>Map</Text>
                    </Button>
                    <Button vertical>
                        <Icon name="list" />
                        <Text>Details</Text>
                    </Button>
                </FooterTab>
            </Footer>
        </Container>
    )
};
