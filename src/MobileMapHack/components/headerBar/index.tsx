import React from "react";
import {Container, Content, Text, Header, Button, Left, Body, Title, Icon, Right} from "native-base";
import Constants from "expo-constants";

export const HeaderBar = (props) => {
    return (
        <Header style={{paddingTop: Constants.statusBarHeight}}>
            <Left>
                <Button
                    transparent
                    onPress={() => props.navigation.openDrawer()}>
                    <Icon name="menu"/>
                </Button>
            </Left>
            <Body>
                <Title>{props.title}</Title>
            </Body>
            <Right />
        </Header>
    )
};